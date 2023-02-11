using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [field: SerializeField] [field: ReadOnly] public SO_Encounter data { get; private set; }

    [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }

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

        this.spriteRenderer.sprite = _data.sprite;
    }

    public void SelectChoice(int _choiceIdx)
    {
        SO_Encounter.S_Choices selected = this.data.choices[_choiceIdx];

        foreach (var item in selected.rewards)
        {
            switch (item.currency)
            {
                case SO_Encounter.E_Currency.Followers:
                    GameManager.UpdateFollowersCount(item.GetCost());
                    break;

                case SO_Encounter.E_Currency.Gold:
                    GameManager.AddGold(item.GetCost());
                    break;

                default:
                    break;
            }
        }

        foreach (var item in selected.losses)
        {
            switch (item.currency)
            {
                case SO_Encounter.E_Currency.Followers:
                    GameManager.RemoveFollowers(item.GetCost());
                    break;

                case SO_Encounter.E_Currency.Gold:
                    GameManager.RemoveGold(item.GetCost());
                    break;

                default:
                    break;
            }
        }

        foreach (var item in selected.actions)
        {
            item?.OnTrigger();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (encounterFlag) return;

        King king = collision.GetComponent<King>();
        if (king == null) return;

        king.SetMovementState(false);

        encounterFlag = true;
        GameManager.currentEncounter = this;

        if (data.dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(data.dialogue, () =>
            {
                UIManager.Instance.SetupEncounterWindow(this.data);
            });
        }
        else UIManager.Instance.SetupEncounterWindow(this.data);

    }
}
