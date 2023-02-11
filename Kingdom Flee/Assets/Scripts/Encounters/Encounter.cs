using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [field: SerializeField] [field: ReadOnly] public SO_Encounter data { get; private set; }

    [field: SerializeField] public Transform assetAnchor { get; private set; }

    private bool encounterFlag = false;

    public static Encounter Create(SO_Encounter _data, Vector2 _position)
    {
        Encounter ec = Instantiate(GameAssets.Instance.EncounterPF, _position, Quaternion.identity)
                       .GetComponent<Encounter>();

        ec.Setup(_data);

        return ec;
    }

    public void Setup(SO_Encounter _data)
    {
        this.data = _data;

        Instantiate(_data.prefabAsset, assetAnchor);
    }

    public void SelectChoice(int _choiceIdx)
    {
        SO_Encounter.S_Choices selected = this.data.choices[_choiceIdx];

        int[] vals = null;
        int valueIndex = 0;
        if (selected.dialogueAfterChoice != null)
        {
            vals = new int[selected.dialogueAfterChoice.dialogueValuesCount];
        }

        int val;
        foreach (var item in selected.rewards)
        {
            val = item.GetCost();

            if (vals != null && valueIndex < vals.Length)
            {
                vals[valueIndex] = val;
                valueIndex++;
            }

            switch (item.currency)
            {
                case SO_Encounter.E_Currency.Followers:
                    GameManager.UpdateFollowersCount(val);
                    
                    break;

                case SO_Encounter.E_Currency.Gold:
                    GameManager.AddGold(val);
                    break;

                default:
                    break;
            }
        }

        foreach (var item in selected.losses)
        {
            val = item.GetCost();

            if (vals != null && valueIndex < vals.Length)
            {
                vals[valueIndex] = val;
                valueIndex++;
            }

            switch (item.currency)
            {
                case SO_Encounter.E_Currency.Followers:
                    GameManager.RemoveFollowers(val);
                    break;

                case SO_Encounter.E_Currency.Gold:
                    GameManager.RemoveGold(val);
                    break;

                default:
                    break;
            }
        }

        foreach (var item in selected.actions)
        {
            item?.OnTrigger();
        }

        if (selected.dialogueAfterChoice != null) DialogueManager.Instance.StartDialogue(selected.dialogueAfterChoice, EndEncouter, vals);
        else EndEncouter();
    }

    private void EndEncouter()
    {
        Destroy(this.gameObject);
        EncountersManager.Instance.SpawnNewEncounter();
        GameManager.Instance.Player.SetMovementState(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (encounterFlag) return;

        King king = collision.GetComponent<King>();
        if (king == null) return;

        king.SetMovementState(false);

        encounterFlag = true;
        GameManager.currentEncounter = this;

        if (data.dialogueBeforeChoice != null)
        {
            DialogueManager.Instance.StartDialogue(data.dialogueBeforeChoice, () =>
            {
                UIManager.Instance.SetupEncounterWindow(this.data);
            });
        }
        else UIManager.Instance.SetupEncounterWindow(this.data);

    }
}
