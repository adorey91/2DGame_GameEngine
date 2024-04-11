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
        Confirmation,
    }

    public GameState gameState;
    private GameState currentState;
    internal GameState stateBeforeOptions;

    [Header("Other Managers")]
    public UIManager _uiManager;
    public LevelManager _levelManager;
    public SoundManager _soundManager;
    public ItemManager _itemManager;
    public QuestManager _questManager;
    public InventoryManager _inventoryManager;

    [Header("Player Settings")]
    public GameObject player;
    public GameObject spawnPoint;
    public Sprite currentSprite;
    private Sprite lastSprite;

    [Header("Controls")]
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private TMP_Text pauseText;

    internal bool isPaused = false;
    private bool volumeLowered = false;


    public void Start()
    {
        gameState = GameState.MainMenu;
    }

    private void StateSwitch(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu: MainMenu(); break;
            case GameState.Gameplay: GamePlay(); break;
            case GameState.Pause: Pause(); break;
            case GameState.Options: Options(); break;
            case GameState.GameComplete: GameComplete(); break;
            case GameState.Credits: Credits(); break;
            case GameState.Dialogue: Dialogue(); break;
            case GameState.Confirmation: Confirmation(); break;
        }
        currentState = gameState;
    }

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
        else if (state == "Confirmation")
            gameState = GameState.Confirmation;
        else if (state == "Dialogue")
            gameState = GameState.Dialogue;
        else
            Debug.Log("State doesnt exist");
    }

    public void EscapeState()
    {
        lastSprite = currentSprite;

        if (currentState == GameState.Gameplay)
            LoadState("Pause");
        else if (currentState == GameState.Pause)
            LoadState("Gameplay");
        else if (currentState == GameState.Options)
            LoadState(stateBeforeOptions.ToString());
    }

    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    #region StateUI-Update
    void MainMenu()
    {
        isPaused = true;
        _uiManager.UI_MainMenu();
        _itemManager.ResetAllItems();
        _questManager.ResetAllQuests();
        _inventoryManager.EmptyInventory();
        _soundManager.MainMenuAudio();
        volumeLowered = false;
    }

    void GamePlay()
    {
        isPaused = false;
        _uiManager.UI_GamePlay();
        currentSprite = lastSprite;
        _soundManager.GameplayAudio();
        volumeLowered = false;
    }

    void Pause()
    {
        isPaused = true;
        _uiManager.UI_Pause();
        if (volumeLowered == false)
        {
            _soundManager.mainAudioSource.volume /= 2;
            volumeLowered = true;
        }
    }

    void Confirmation()
    {
        isPaused = true; // using this here to make sure player can't interact with things while on this screen
        _uiManager.UI_Confirmation();
    }


    void GameComplete()
    {
        isPaused = true; // using this here to make sure player can't interact with things while on this screen
        _uiManager.UI_GameCompleted();
        _soundManager.GameWinAudio();
    }

    void Credits()
    {
        _uiManager.UI_Credits();
    }

    void Options()
    {
        isPaused = true; // using this here to make sure player can't interact with things while on this screen
        _uiManager.UI_Options();
    }

    void Dialogue()
    {
        _uiManager.UI_Dialogue();
        if (volumeLowered == false)
        {
            _soundManager.mainAudioSource.volume /= 2;
            volumeLowered = true;
        }
    }
    #endregion

    public void MovePlayerToSpawnLocation(string spawn)
    {
        if (spawn != "Gameplay_DarkCastle")
            spawnPoint = GameObject.Find("SpawnPoint");
        else
            spawnPoint = GameObject.Find("SpawnPoint_ReturnFromDarkCastle");

        player.transform.position = spawnPoint.transform.position;
    }
}