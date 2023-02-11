using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter Action", menuName = "Scriptable/Encounters")]
public abstract class SO_EncounterActions : ScriptableObject
{
    public abstract void OnTrigger();
}