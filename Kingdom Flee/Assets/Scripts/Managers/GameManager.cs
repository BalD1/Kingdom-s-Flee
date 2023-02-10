using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private float deltaTime;
    private int fps;

    public const string MAIN_SCENE = "MainScene";
    public const string MAIN_MENU = "MainMenu";

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

    private void InitState()
    {
        if (SceneManager.GetActiveScene().name == MAIN_MENU) gameState = E_GameStates.MainMenu;
        else gameState = E_GameStates.InGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseHandler();

        CalculateFPS();
    }

    private void CalculateFPS()
    {
        deltaTime += Time.deltaTime;
        deltaTime /= 2;
        fps = (int)Mathf.Round(1 / deltaTime);
    }

    public void PauseHandler()
    {
        if (GameState == E_GameStates.InGame) GameState = E_GameStates.Pause;
        else if (GameState == E_GameStates.InGame) GameState = E_GameStates.InGame;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 0, 150, 25), "GameState : " + GameState.ToString());
        GUI.Label(new Rect(10, 25, 80, 25), fps + " FPS");
    }
}
