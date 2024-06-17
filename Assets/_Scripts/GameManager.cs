using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Gamestate
    {
        MainMenu,
        Gameplay,
        Pause,
        Options,
        EndGame,
        Dialogue,
    }

    public Gamestate gameState;
    public Gamestate stateBeforeOptions;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        SetState(Gamestate.MainMenu);
    }

    private void SetState(Gamestate state)
    {
        gameState = state;

        switch (state)
        {
            case Gamestate.MainMenu: MainMenu(); break;
            case Gamestate.Gameplay: Gameplay(); break;
            case Gamestate.Pause: Pause(); break;
            case Gamestate.Options: Options(); break;
            case Gamestate.EndGame: GameCompleted(); break;
            case Gamestate.Dialogue: Dialogue(); break;
        }

    }

    public void LoadState(string state)
    {
        if (Enum.TryParse(state, out Gamestate gamestate))
            LoadState(gamestate);
        else
            Debug.Log("Invalid State " + state);
    }

    private void LoadState(Gamestate state)
    {
        if (state == Gamestate.Options)
            stateBeforeOptions = gameState;

        SetState(state);
    }


    private void MainMenu()
    {
        questManager.ResetAllQuests();
        inventoryManager.EmptyInventory();

        uiManager.UI_MainMenu();
        // sound manager main menu audio
    }

    private void Gameplay()
    {
        uiManager.UI_Gameplay();
    }

    private void Pause()
    {
        uiManager.UI_Pause();
    }

    private void Dialogue()
    {
        uiManager.UI_Dialogue();
    }

    private void GameCompleted()
    {
        uiManager.UI_GameCompleted();
    }

    private void Options()
    {
        uiManager.UI_Options();
    }
}
