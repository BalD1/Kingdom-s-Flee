using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.D_gameStateChange += WindowsManager;
    }

    public void WindowsManager()
    {
        GameManager.E_GameStates newState = GameManager.Instance.GameState;

        switch (newState)
        {
            case GameManager.E_GameStates.InGame:

                break;

            case GameManager.E_GameStates.Pause:
                break;

            default:
                Debug.LogError(newState + " was not found in switch statement.", Instance.gameObject);
                break;
        }
    }
}
