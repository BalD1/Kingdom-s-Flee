using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable/Dialogue")]
public class SO_SingleDialogue : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }

    [field: SerializeField] public bool ignoreGameState { get; private set; }

    [field: SerializeField] public DialogueLine[] dialogueLines { get; private set; }

    [field: SerializeField] public int dialogueValues { get; private set; }


    [System.Serializable]
    public struct DialogueLine
    {
        [field: SerializeField] public string speakerName { get; private set; }
        [field: SerializeField] [field: TextArea] public string textLine { get; private set; }
        [field: SerializeField] public DialogueEffect[] effects { get; private set; }
    }

    [System.Serializable]
    public struct DialogueEffect
    {
        [field: SerializeField] public E_Effects effect { get; private set; }
        [field: SerializeField] public float value { get; private set; }
    }

    public enum E_Effects
    {
        ProgressiveReveal,
        PauseOnIndex,
    }
}
