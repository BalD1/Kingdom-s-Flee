using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "ButtonsPreset", menuName = "Scriptable/UI/ButtonsPreset")]
public class SO_ButtonsPreset : ScriptableObject
{
    [field: SerializeField] public ColorBlock buttonColors { get; private set; }

    [field: SerializeField] public Type[] componentsToAdd { get; private set; }
    [field: SerializeField] public UnityAction[] onClickActions { get; private set; }

    [InspectorButton(nameof(SetupComponentsArray), ButtonWidth = 150)]
    [SerializeField] private bool setupComponentsArray;

    private void SetupComponentsArray()
    {
        componentsToAdd = new Type[1];
        componentsToAdd[0] = typeof(ButtonsTweenBase);

        onClickActions = new UnityAction[1];
        onClickActions[0] = delegate { OnClickPlaySFX("S_Click"); };
    }

    public void SetupButton(Button button)
    {
        button.colors = buttonColors;

        if (componentsToAdd == null) SetupComponentsArray();

        foreach (var item in componentsToAdd)
        {
            if (button.gameObject.GetComponent(item) == null)
                button.gameObject.AddComponent(item);
        }

        foreach (var item in onClickActions)
        {
            button.onClick.RemoveListener(item);
            button.onClick.AddListener(item);
        }
    }

    public void OnClickPlaySFX(string args)
    {
        AudioManager.Instance.Play2DSFX(args);
    }
}