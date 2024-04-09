using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Pause,
        Options,
        Credits,
        GameComplete,
        Dialogue,
    }

    public GameState gameState;
    GameState currentState;
    internal GameState stateBeforeOptions;

    [Header("Other Managers")]
    public UIManager _uiManager;
    public LevelManager _levelManager;
    public SoundManager _soundManager;

    public GameObject spawnPoint;
    public GameObject player;

    [Header("Controls")]
    public KeyCode interactKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode rollKey = KeyCode.R;
    public KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] TMP_Text interactText;
    [SerializeField] TMP_Text pauseText;


    bool volumeLowered = false;


    public void Start()
    {
        gameState = GameState.MainMenu;
        StateSwitch();

        interactText.text = $"{interactKey}";
        pauseText.text = $"{pauseKey}";
    }

    public void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            EscapeState();

        if (gameState != currentState)
            StateSwitch();

    }

    public void StateSwitch()
    {
        switch (gameState)
        {
            case GameState.MainMenu: MainMenu(); break;
            case GameState.Gameplay: GamePlay(); break;
            case GameState.Pause: Pause(); break;
            case GameState.Options: Options(); break;
            case GameState.GameComplete: GameComplete(); break;
            case GameState.Credits: Credits(); break;
            case GameState.Dialogue: Dialogue(); break;
        }
        currentState = gameState;
    }

    void EscapeState()
    {
        if (currentState == GameState.Gameplay)
            LoadState("Pause");
        else if (currentState == GameState.Pause)
            LoadState("Gameplay");
        else if (currentState == GameState.Options)
            LoadState(stateBeforeOptions.ToString());
    }

    #region LoadState/Quit
    public void LoadState(string state)
    {
        if (state == "Options")
        {
            stateBeforeOptions = currentState;
            gameState = GameState.Options;
        }
        else if (state == "MainMenu")
            gameState = GameState.MainMenu;
        else if (state == "Pause")
            gameState = GameState.Pause;
        else if (state == "Gameplay")
            gameState = GameState.Gameplay;
        else if (state == "Credits")
            gameState = GameState.Credits;
        else if (state == "GameComplete")
            gameState = GameState.GameComplete;
        else if (state == "BeforeOptions")
            gameState = stateBeforeOptions;
        else if (state == "Dialogue")
            gameState = GameState.Dialogue;
        else
            Debug.Log("State doesnt exist");
    }

    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
    #endregion

    #region StateUI-Update
    void MainMenu()
    {
        _uiManager.UI_MainMenu();
        _soundManager.MainMenuAudio();
        volumeLowered = false;
    }

    void GamePlay()
    {
        _uiManager.UI_GamePlay();
        _soundManager.GameplayAudio();
        volumeLowered = false;
    }

    void Pause()
    {
        _uiManager.UI_Pause();
        if (volumeLowered == false)
        {
            _soundManager.audioSource.volume = _soundManager.audioSource.volume / 2;
            volumeLowered = true;
        }
    }

    void GameComplete()
    {
        _uiManager.UI_GameCompleted();
        _soundManager.GameWinAudio();
    }

    void Credits()
    {
        _uiManager.UI_Credits();
    }

    void Options()
    {
        _uiManager.UI_Options();
    }

    void Dialogue()
    {
        _uiManager.UI_Dialogue();
        if (volumeLowered == false)
        {
            _soundManager.audioSource.volume = _soundManager.audioSource.volume / 2;
            volumeLowered = true;
        }
    }
    #endregion

    public void MovePlayerToSpawnLocation(string spawn)
    {
        if(spawn != "Gameplay_DarkCastle")
            spawnPoint = GameObject.Find("SpawnPoint");
        else
            spawnPoint = GameObject.Find("SpawnPoint_ReturnFromDarkCastle");

        player.transform.position = spawnPoint.transform.position;
    }
}