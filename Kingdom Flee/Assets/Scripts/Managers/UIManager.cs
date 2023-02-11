using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SO_Encounter;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject buttonChoicePF;
    [SerializeField] private GameObject linePF;

    [SerializeField] private GameObject encountersPanel;

    [SerializeField] private RectTransform choicesButtonPanel;

    private ChoiceButton[] currentChoiceButtons;

    [SerializeField] private RectTransform gainsParent;
    [SerializeField] private RectTransform lossesParent;

    [SerializeField] [ReadOnly] private S_Choices[] currentChoices;
    [SerializeField] [ReadOnly] private int currentChoiceIndex = 0;

    [SerializeField] private TextMeshProUGUI followersCount;
    [SerializeField] private TextMeshProUGUI goldCount;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.D_gameStateChange += WindowsManager;
    }

    public void WindowsManager()
    {
        GameManager.E_GameStates newState = GameManager.Instance.GameState;

        switch (newState)
        {
            case GameManager.E_GameStates.InGame:

                break;

            case GameManager.E_GameStates.Pause:
                break;

            default:
                Debug.LogError(newState + " was not found in switch statement.", Instance.gameObject);
                break;
        }
    }

    public void UpdateFollowersCount()
    {
        followersCount.text = "x " + GameManager.FollowersCount;
    }

    public void UpdateGoldCount()
    {
        goldCount.text = "x " + GameManager.GoldCount;
    }

    public void SetupEncounterWindow(SO_Encounter _data)
    {
        if (_data == null) return;

        foreach (Transform item in choicesButtonPanel)
        {
            Destroy(item.gameObject);
        }

        encountersPanel.SetActive(true);

        currentChoices = _data.choices;
        currentChoiceIndex = 0;

        int choicesCount = _data.choices.Length;

        currentChoiceButtons = new ChoiceButton[choicesCount];

        for (int i = 0; i < choicesCount; i++)
        {
            GameObject newButton = Instantiate(buttonChoicePF, choicesButtonPanel);

            SO_Encounter.S_Choices currentChoice = _data.choices[i];

            ChoiceButton cb = newButton.GetComponent<ChoiceButton>();
            cb.Setup(currentChoice, i);

            currentChoiceButtons[i] = cb;
        }

        currentChoiceButtons[0].GetComponent<Button>().Select();

        DisplayChoice(0);
    }

    public void CloseEncounterWindow()
    {
        encountersPanel.SetActive(false);
        currentChoices = null;
    }

    public void DisplayChoice(int choiceIdx)
    {
        if (currentChoices == null || currentChoices.Length <= 0) return;
        if (choiceIdx > currentChoices.Length) return;
        if (choiceIdx < 0) return;

        SetupGainLossPanel(currentChoices[choiceIdx].rewards, gainsParent);
        SetupGainLossPanel(currentChoices[choiceIdx].losses, lossesParent);
    }

    private void SetupGainLossPanel(S_CostCurrency[] _data, RectTransform parent, bool cleanup = true)
    {
        if (cleanup)
        {
            foreach (Transform item in parent) Destroy(item.gameObject);
        }

        foreach (var item in _data)
        {
            GameObject newLine = Instantiate(linePF, parent);
            newLine.GetComponent<Line>().Setup(item.CurrencyImage(), item.cost);
        }
    }
}
