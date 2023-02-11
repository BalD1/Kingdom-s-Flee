using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowerColors", menuName = "Scriptable/Entities/Followers/Colors")]
public class SO_FollowerColors : ScriptableObject
{
    [field: SerializeField] public Gradient[] bodyColors { get; private set; }
    [field: SerializeField] public Gradient[] headColors { get; private set; }

    public Color GetRandomBodyColor()
    {
        return bodyColors.RandomElement().Evaluate(Random.Range(0, 1f));
    }

    public Color GetRandomHeadColor()
    {
        return headColors.RandomElement().Evaluate(Random.Range(0, 1f));
    }
}