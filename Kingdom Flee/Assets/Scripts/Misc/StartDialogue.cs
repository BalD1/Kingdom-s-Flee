using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [ListToPopup(typeof(DialogueManager), nameof(DialogueManager.DialogueNamesList))]
    [SerializeField] private string startDialogue;

    private void Start()
    {
        bool res = DialogueManager.Instance.TryStartDialogue(startDialogue, () =>
        {
            GameManager.Instance.Player.SetMovementState(true);
        });

        if (!res) GameManager.Instance.Player.SetMovementState(true);
    }
}
