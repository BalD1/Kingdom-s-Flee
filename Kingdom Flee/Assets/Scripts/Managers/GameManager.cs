using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public const string MAIN_SCENE = "MainScene";
    public const string MAIN_MENU = "MainMenu";

    public static int FollowersCount { get; private set; }
    public static int GoldCount { get; private set; }

    public static Encounter currentEncounter = null;

    [field: SerializeField] public King Player { get; private set; }

    #region GameState

    public enum E_GameStates
    {
        MainMenu,
        InGame,
        Pause,
    }

    private E_GameStates gameState;
    public E_GameStates GameState
    {
        get => gameState;
        set
        {
            gameState = value;

            switch (gameState)
            {
                case E_GameStates.MainMenu:
                    break;

                case E_GameStates.InGame:
                    break;

                case E_GameStates.Pause:
                    break;

                default:
                    Debug.LogError(value + " was not found in switch statement.", Instance.gameObject);
                    break;
            }

            D_gameStateChange?.Invoke();
        }
    }

    public delegate void D_GameStateChange();
    public D_GameStateChange D_gameStateChange; 

    #endregion

    protected override void Awake()
    {
        base.Awake();

        if (Player == null) SearchForPlayer();

        InitState();
    }

    private void Start()
    {
        AddGold(10);
    }

    private void SearchForPlayer()
    {
        Debug.LogError("Player Object was not set in GameManager", this.gameObject);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        King player;

        foreach (var item in objs)
        {
            player = item.GetComponent<King>();

            if (player != null)
            {
                Player = player;
                return;
            }
        }
    }

    public static void AddFollowers(int amount)
    {
        UpdateFollowersCount(amount);
    }
    public static void RemoveFollowers(int amount)
    {
        UpdateFollowersCount(-amount);
    }
    private static void UpdateFollowersCount(int amount)
    {
        FollowersCount += amount;
        UIManager.Instance.UpdateFollowersCount();
    }

    public static void AddGold(int amount)
    {
        UpdateGoldCount(amount);
    }
    public static void RemoveGold(int amount)
    {
        UpdateGoldCount(-amount);
    }
    private static void UpdateGoldCount(int amount)
    {
        GoldCount += amount;
        UIManager.Instance.UpdateGoldCount();
    }

    private void InitState()
    {
        if (SceneManager.GetActiveScene().name == MAIN_MENU) gameState = E_GameStates.MainMenu;
        else gameState = E_GameStates.InGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseHandler();
    }

    public void PauseHandler()
    {
        if (GameState == E_GameStates.InGame) GameState = E_GameStates.Pause;
        else if (GameState == E_GameStates.InGame) GameState = E_GameStates.InGame;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 0, 150, 25), "GameState : " + GameState.ToString());
    }
}
