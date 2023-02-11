using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Scriptable/Encounters")]
public class SO_Encounter : ScriptableObject
{
    [field: SerializeField] public Sprite sprite { get; private set; }

    [field: SerializeField] public S_Choices[] choices { get; private set; }

    [field: SerializeField] public SO_SingleDialogue dialogue { get; private set; }

    public enum E_Currency
    {
        Followers,
        Gold,
    }

    [System.Serializable]
    public struct S_CostCurrency
    {
        [field: SerializeField] public bool randomCost { get; private set; }
        [field: SerializeField] public int cost { get; private set; }
        [field: SerializeField] public int maxCost { get; private set; }
        [field: SerializeField] public E_Currency currency { get; private set; }

        public int GetCost()
        {
            return randomCost ? Random.Range(cost, maxCost) :
                                cost;
        }

        public Sprite CurrencyImage()
        {
            switch (currency)
            {
                case E_Currency.Followers:
                    return GameAssets.Instance.followersSprite;

                case E_Currency.Gold:
                    return GameAssets.Instance.goldSprite;

                default:
                    return GameAssets.Instance.followersSprite;
            }
        }
    }

    [System.Serializable]
    public struct S_Choices
    {
        [field: SerializeField] public string choiceName { get; private set; }
        [field: SerializeField] public S_CostCurrency[] losses { get; private set; }
        [field: SerializeField] public S_CostCurrency[] rewards { get; private set; }
        [field: SerializeField] public SO_EncounterActions[] actions { get; private set; }
    }

}