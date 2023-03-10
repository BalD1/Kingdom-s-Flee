using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI textMesh;

    private int index;

    private SO_Encounter.S_Choices choice;

    public void Setup(SO_Encounter.S_Choices _choice, int _index, bool canClic = true)
    {
        choice = _choice;

        textMesh.text = choice.choiceName;

        index = _index;

        this.button.interactable = canClic;
    }

    public void OnClick()
    {
        UIManager.Instance.CloseEncounterWindow();

        GameManager.currentEncounter.SelectChoice(this.index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        UIManager.Instance.DisplayChoice(index);
    }
}
