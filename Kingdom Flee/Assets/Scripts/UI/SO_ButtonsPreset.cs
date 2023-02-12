using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "ButtonsPreset", menuName = "Scriptable/UI/ButtonsPreset")]
public class SO_ButtonsPreset : ScriptableObject
{
    [field: SerializeField] public ColorBlock buttonColors { get; private set; }

    [field: SerializeField] public Type[] componentsToAdd { get; private set; }

    [InspectorButton(nameof(SetupComponentsArray), ButtonWidth = 150)]
    [SerializeField] private bool setupComponentsArray;

    private void SetupComponentsArray()
    {
        componentsToAdd = new Type[1];
        componentsToAdd[0] = typeof(ButtonsTweenBase);
    }

    public void SetupButton(Button button)
    {
        button.colors = buttonColors;

        foreach (var item in componentsToAdd)
        {
            button.gameObject.AddComponent(item);
        }
    }
}