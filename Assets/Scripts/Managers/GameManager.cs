using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        GamePlay,
        Pause,
        Options,
        Credits,
        GameComplete,
        Dialogue,
        Confirmation,
        Controls
    }

    public GameState gameState;
    public GameState stateBeforeSettings;

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
    //public Sprite currentSprite;
    //private Sprite lastSprite;

    [Header("Controls")]
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private TMP_Text pauseText;

    internal bool isPaused = false;
    private bool volumeLowered = false;

    private void Start()
    {
        SetState(GameState.MainMenu);
    }

    private void SetState(GameState state)
    {
        gameState = state;
        
        switch (state)
        {
            case GameState.MainMenu: MainMenu(); break;
            case GameState.GamePlay: GamePlay(); break;
            case GameState.Pause: Pause(); break;
            case GameState.Confirmation: Confirmation(); break;
            case GameState.Dialogue: Dialogue(); break;
            case GameState.GameComplete: GameComplete(); break;
            case GameState.Options: Options(); break;
            case GameState.Credits: Credits(); break;
            case GameState.Controls: Controls(); break;
            default: Debug.LogError("Unknown State: "+ state); break;
        }
    }

    public void LoadState(string state)
    {
        if (Enum.TryParse(state, out GameState gameState))
            LoadState(gameState);
        else
            Debug.LogError("Invalid state: " + state);
    }


    private void LoadState(GameState state)
    {
        if (state == GameState.Options || state == GameState.Controls)
            stateBeforeSettings = gameState;

        SetState(state);
    }

    public void EscapeState()
    {
        if (gameState == GameState.GamePlay)
            LoadState(GameState.Pause);
        else if (gameState == GameState.Pause)
            LoadState(GameState.GamePlay);
        else if (gameState == GameState.Options || gameState == GameState.Controls)
            LoadState(stateBeforeSettings);
    }

    public void GameCompleted()
    {
        LoadState(GameState.GameComplete);
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
        PlayerPrefs.DeleteAll();
    }

    void GamePlay()
    {
        isPaused = false;
        _uiManager.UI_GamePlay();
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

    void Controls()
    {
        isPaused = true;
        _uiManager.UI_Controls();
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
}