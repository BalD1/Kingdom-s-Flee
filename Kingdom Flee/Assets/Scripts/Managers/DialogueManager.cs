using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance
    {
        get => instance;
    }

    public static List<string> DialogueNamesList;

    [SerializeField] private CanvasGroup dialogueContainer;

    [SerializeField] private float leanFadeTime = .2f;

    [SerializeField] private float allowSkip_COOLDOWN = .15f;
    private float allowSkip_TIMER;

    [SerializeField] private float onStartSkipWait_DURATION = .3f;
    private float onStartSkipWait_TIMER;

    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI pressKeyToContinue;

    [SerializeField] private AudioSource audioSource;
    [field: SerializeField] public SO_SingleDialogue[] Dialogues { get; private set; }

#if UNITY_EDITOR
    [InspectorButton(nameof(UpdateDialogueList), ButtonWidth = 300)][SerializeField] private bool updateNamesList;

    [ListToPopup(typeof(DialogueManager), nameof(DialogueNamesList))]
    [SerializeField] private string dialogueNamePopupTest; 
#endif

    [SerializeField] [ReadOnly] private SO_SingleDialogue currentDialogue;
    [SerializeField] [ReadOnly] private SO_SingleDialogue.DialogueLine currentLine;
    [SerializeField] [ReadOnly] private int currentLineIndex = -1;

    private string currentLineText;

    private string[] splittedLine;

    private int[] values;
    private int valuesIndex;

    private Action endDialogueAction;

    private Coroutine revealCoroutine;

    private int unfinishedEffectsCount;
    private int UnfinishedEffectsCount
    {
        get => unfinishedEffectsCount;
        set
        {
            unfinishedEffectsCount = value;
            if (unfinishedEffectsCount <= 0) OnIsReadyToShowNextLine();
        }
    }

    private Queue<int> pauseOnIndexQueue = new Queue<int>();

    private void Awake()
    {
        instance = this;

        SceneManager.sceneUnloaded += OnSceneReset;
    }

    private void OnSceneReset(Scene scene)
    {
        ResetDialogue();
    }

    private void Start()
    {
        GameManager.Instance.D_gameStateChange += GameStateWatcher;
    }

    private void Update()
    {
        if (allowSkip_TIMER > 0) allowSkip_TIMER -= Time.deltaTime;
        if (onStartSkipWait_TIMER > 0) onStartSkipWait_TIMER -= Time.deltaTime;
    }

    private void GameStateWatcher()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameManager.E_GameStates.InGame:
                dialogueContainer.gameObject.SetActive(true);
                break;

            case GameManager.E_GameStates.Pause:
                dialogueContainer.gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// <para> Searchs in the list for the "<paramref name="searchedID"/>" dialogue, then plays it if found. </para>
    /// <para> Can trigger "<paramref name="actionAtDialogueEnd"/>" at the end of the dialogue. </para>
    /// </summary>
    /// <param name="searchedID"></param>
    /// <param name="actionAtDialogueEnd"></param>
    /// <returns>Returns <see langword="true"/> if the dialogue was found,  <see langword="false"/> otherwise.</returns>
    public bool TryStartDialogue(string searchedID, Action actionAtDialogueEnd = null)
    {
        foreach (var item in Dialogues)
        {
            if (item.ID == searchedID)
            {
                StartDialogue(item);

                endDialogueAction = actionAtDialogueEnd;
                return true;
            }
        }

#if UNITY_EDITOR
        Debug.LogErrorFormat($"{searchedID} was not found in {Dialogues} array."); 
#endif
        return false;
    }

    /// <summary>
    /// Starts the dialogue "<paramref name="dialogue"/>", activating "cinematic" mode.
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(SO_SingleDialogue dialogue, Action actionAtDialogueEnd = null, int[] _values = null)
    {
        GameManager.Instance.Player.playerInputs.D_anyKeyPressed += TryNextLine;
        UIManager.Instance.HUD.SetActive(false);

        onStartSkipWait_TIMER = onStartSkipWait_DURATION;

        //PostproManager.Instance.SetBlurState(true);

        dialogueText.text = "";
        speakerNameText.text = "";

        values = _values;
        valuesIndex = 0;

        currentDialogue = dialogue;

        // Fades in the dialogue container, then shows the first line
        dialogueContainer.LeanAlpha(1f, leanFadeTime)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => ShowNextLine());

        dialogueContainer.blocksRaycasts = true;

        if (actionAtDialogueEnd != null)
            endDialogueAction = actionAtDialogueEnd;
    }

    /// <summary>
    /// <para> Shows the next line. If the current one's effects aren't finished, end them.</para>
    /// </summary>
    public void TryNextLine()
    {
        if (GameManager.Instance.GameState == GameManager.E_GameStates.Pause) return;
        if (onStartSkipWait_TIMER > 0) return;
        if (currentDialogue == null || currentLine.textLine == null) return;
        if (UnfinishedEffectsCount > 0 && allowSkip_TIMER <= 0)
        {
            ForceStopEffects();

            UnfinishedEffectsCount = 0;

            return;
        }

        ShowNextLine();
    }

    /// <summary>
    /// Shows the next dialogue line, and plays it's effects if it haves any.
    /// </summary>
    private void ShowNextLine()
    {
        if (currentLineIndex == -1) currentLineIndex = 0;
        if (currentLineIndex >= currentDialogue.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        if (revealCoroutine != null)  StopCoroutine(revealCoroutine);

        pauseOnIndexQueue.Clear();
        UnfinishedEffectsCount = 0;

        currentLine = currentDialogue.dialogueLines[currentLineIndex];

        currentLineText = currentLine.textLine;

        Color lineColor = currentLine.textColor;
        lineColor.a = 1;
        dialogueText.color = lineColor;

        if (values != null && valuesIndex < values.Length)
        {
            string sub = "{" + valuesIndex + "}";
        
            while (currentLineText.Contains(sub) && valuesIndex < values.Length)
            {
                currentLineText = currentLineText.Replace(sub, values[valuesIndex].ToString());

                valuesIndex++;
                 sub = "{" + valuesIndex + "}";
            }
        }

        dialogueText.text = currentLineText;
        speakerNameText.text = currentLine.speakerName;

        foreach (var item in currentLine.effects)
        {
            ManageEffects(item);
        }

        // If there is no effects, instantly ready the next line.
        if (UnfinishedEffectsCount <= 0)
            OnIsReadyToShowNextLine();
        else
            OnCantShowNextLine();

        currentLineIndex++;
    }

    private void OnCantShowNextLine()
    {
        allowSkip_TIMER = allowSkip_COOLDOWN;
        LeanTween.value(pressKeyToContinue.alpha, 0, .2f).setOnUpdate(
            (float val) =>
            {
                pressKeyToContinue.alpha = val;
            }).setIgnoreTimeScale(true);
    }
    private void OnIsReadyToShowNextLine()
    {
        LeanTween.value(pressKeyToContinue.alpha, 1, .2f).setOnUpdate(
            (float val) =>
            {
                pressKeyToContinue.alpha = val;
            }).setIgnoreTimeScale(true).setOnComplete(() => LeanTween.delayedCall(1, () => PressKeyTextVisibleEffects()));
    }

    /// <summary>
    /// Plays the animation of the "press key" text.
    /// </summary>
    private void PressKeyTextVisibleEffects()
    {
        if (pressKeyToContinue == null) return;
        LeanTween.delayedCall(3, () => {

            if (pressKeyToContinue == null) return;
            if (pressKeyToContinue.alpha <= 0) return;

            LeanTween.scale(pressKeyToContinue.rectTransform, Vector3.one * 1.2f, 0.5f)
            .setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
            {
                LeanTween.scale(pressKeyToContinue.rectTransform, Vector3.one, 0.5f);
            });

        }).setRepeat(-1);
    }

    /// <summary>
    /// Ends the dialogue, removing every cinematic effects and giving back player's controls.
    /// </summary>
    public void EndDialogue()
    {
        audioSource?.Stop();

        dialogueContainer.LeanAlpha(0, leanFadeTime)
            .setIgnoreTimeScale(true)
            .setOnComplete(
            () =>
            {
                endDialogueAction?.Invoke();
                ResetDialogue();
            });

        dialogueContainer.blocksRaycasts = false;

        GameManager.Instance.Player.playerInputs.D_anyKeyPressed -= TryNextLine;
        UIManager.Instance.HUD.SetActive(true);

        //PostproManager.Instance.SetBlurState(true);
    }

    private void ResetDialogue()
    {
        UnfinishedEffectsCount = 0;
        currentLineIndex = -1;
        pressKeyToContinue.alpha = 0;
        endDialogueAction = null;
        pauseOnIndexQueue.Clear();
    }

    private void ManageEffects(SO_SingleDialogue.DialogueEffect lineEffect)
    {
        switch (lineEffect.effect)
        {
            case SO_SingleDialogue.E_Effects.ProgressiveReveal:
                UnfinishedEffectsCount++;
                revealCoroutine = StartCoroutine(Reveal(lineEffect.value == 0 ? 0.025f : lineEffect.value));
                break;

            case SO_SingleDialogue.E_Effects.PauseOnIndex:
                pauseOnIndexQueue.Enqueue((int)lineEffect.value);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// <para> Progressive Reveal text effect : makes every dialogue characters appear progressivly, instead of immediatly. </para>
    /// <para> Can play Pause On Index effects, which pauses the text on the character's index for 1 second.</para>
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Reveal(float time)
    {
        dialogueText.ForceMeshUpdate();

        audioSource.clip = currentLine.sfx != null ? currentLine.sfx : AudioManager.Instance.GetClip(AudioManager.E_ClipsTags.S_DialogueBase);
        audioSource.Play();

        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        int counter = 0;

        bool loop = true;

        while(loop)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            if (pauseOnIndexQueue.Count > 0) 
            {
                if (visibleCount == pauseOnIndexQueue.Peek())
                {
                    pauseOnIndexQueue.Dequeue();
                    audioSource.Stop();
                    yield return new WaitForSeconds(1);
                    audioSource.Play();
                }
            }

            dialogueText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                loop = false;
                revealCoroutine = null;
                UnfinishedEffectsCount--;
                audioSource.Stop();
            }

            counter++;

            yield return new WaitForSeconds(time);
        }
    }

    private void ForceStopEffects()
    {
        StopCoroutine(revealCoroutine);
        dialogueText.ForceMeshUpdate();

        audioSource.Stop();

        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        dialogueText.maxVisibleCharacters = totalVisibleCharacters;
    }

    public void UpdateDialogueList()
    {
        DialogueNamesList = new List<string>();
        foreach (var item in Dialogues)
        {
            DialogueNamesList.Add(item.ID);
        }
    }

    public static void SearchAndUpdateDialogueList()
    {
        GameObject.FindObjectOfType<DialogueManager>().UpdateDialogueList();
    }
}
