using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "Scriptable/Entities/Stats")]
public class SO_EntityStats : ScriptableObject
{
    [field: SerializeField] public float speed { get; private set; }
}