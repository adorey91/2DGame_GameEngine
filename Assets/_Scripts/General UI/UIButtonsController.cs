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
    [SerializeField] private ControlBindingsManager controlBindingsManager;

    [Header("Buttons for CreditsUI")]
    [SerializeField] private GameObject quitButton;

    [Header("Buttons for Confirmation UI")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TMP_Text confirmationText;

    private void Start()
    {
        controlBindingsManager.KeyboardBindingsActive();
    }

    public void QuitCreditsButton(bool isEnd)
    {
        quitButton.SetActive(isEnd);
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
