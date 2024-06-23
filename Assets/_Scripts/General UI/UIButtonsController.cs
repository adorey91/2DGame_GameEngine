using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Scripts Needed for Buttons")]
    [SerializeField] private ControlBindingsManager controlBindingsManager;

    [Header("Buttons for MainMenu")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Buttons for Pause")]
    [SerializeField] private Button resumePauseButton;
    [SerializeField] private Button optionsPauseButton;
    [SerializeField] private Button mainMenuPauseButton;
    [SerializeField] private Button quitPauseButton;

    [Header("Buttons for Options")]
    [SerializeField] private Button returnOptionsButton;
    [SerializeField] private Button controlsOptionsButton;
   
    [Header("Buttons for Controls")]
    [SerializeField] private Button resetAllControlsButton;
    [SerializeField] private Button backControlsButton;

    [Header("Buttons for Credits")]
    [SerializeField] private Button continueCreditsButton;

    [Header("Buttons for End Game")]
    [SerializeField] private Button menuEndGameButton;
    [SerializeField] private Button quitEndGameButton;

    [Header("Buttons for Confirmation UI")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TMP_Text confirmationText;

    [Header("Buttons for Dialogue UI")]
    [SerializeField] private Button nextLineButton;


    private void Start()
    {
        SetButtons();
    }

    private void SetButtons()
    {
        // buttons for main menu
        playButton.onClick.AddListener(() => levelManager.LoadScene("Gameplay_field"));
        optionsButton.onClick.AddListener(() => gameManager.LoadState("Options"));
        creditsButton.onClick.AddListener(() => uiManager.UI_Credits());
        quitButton.onClick.AddListener(() => uiManager.UI_Confirmation("quit"));

        // buttons for pause menu
        resumePauseButton.onClick.AddListener(() => gameManager.EscapeState());
        optionsPauseButton.onClick.AddListener(() => gameManager.LoadState("Options"));
        mainMenuPauseButton.onClick.AddListener(() => uiManager.UI_Confirmation("mainMenu"));
        quitPauseButton.onClick.AddListener(() => uiManager.UI_Confirmation("quit"));

        // buttons for options
        returnOptionsButton.onClick.AddListener(() => gameManager.EscapeState());
        controlsOptionsButton.onClick.AddListener(() => uiManager.UI_Controls());

        // buttons for controls
        resetAllControlsButton.onClick.AddListener(() => controlBindingsManager.ResetAllBindings());
        backControlsButton.onClick.AddListener(() => uiManager.UI_Options());

        // button for credits
        continueCreditsButton.onClick.AddListener(() => gameManager.LoadState("MainMenu"));

        // buttons for end game
        menuEndGameButton.onClick.AddListener(() => gameManager.LoadState("MainMenu"));
        quitEndGameButton.onClick.AddListener(() => uiManager.UI_Confirmation("quit"));

        // buttons for dialogue
        nextLineButton.onClick.AddListener(() => dialogueManager.DisplayNextSentence());
    }

    public void SetConfirmation(string word)
    {
        switch(word)
        {
            case "quit":
                confirmationText.text = "Are you sure you want to quit?";
                yesButton.onClick.AddListener(() => Application.Quit());
                break;
            case "mainMenu":
                confirmationText.text = "Are you sure you want to go to Main Menu? All progress will be lost.";
                yesButton.onClick.AddListener(() => levelManager.LoadScene("MainMenu"));
                break;
        }
    }
}
