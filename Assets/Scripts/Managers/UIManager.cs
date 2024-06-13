using UnityEngine;
using System;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject menuUI;
    public GameObject pauseUI;
    public GameObject gameUI;
    public GameObject optionsUI;
    public GameObject creditsUI;
    public GameObject gameCompletedUI;
    public GameObject dialogueUI;
    public GameObject confirmationUI;
    public GameObject controlsUI;
    public ButtonSelect buttonSelect;

    [Header("Controls UI Objects")]
    public GameObject keyboardBindings;
    public GameObject gamepadBindings;
    public Button keyboardButton;
    public Button gamepadButton;
    public TMP_Text controlsText;


    [Header("Player Settings")]
    public GameObject player;
    public GameObject playerSprite;
    public SpriteRenderer playerThought;
    public TMP_Text playerThoughtText;
    bool isThoughtEnabled;

    [Header("Inventory")]
    [SerializeField] InventorySlotUI[] itemUISlots;

    [Header("DialogueFade")]
    [SerializeField] float fadeSpeed;
    private CanvasGroup canvGroup;

    private void Awake()
    {
        canvGroup = dialogueUI.GetComponent<CanvasGroup>();
    }

    public void UI_MainMenu()
    {
        PlayerNGame(false, CursorLockMode.None, true, 0f);
        playerThought.enabled = false;
        playerThoughtText.enabled = false;
        SetUIActive(menuUI);
        buttonSelect.Change("MainMenu");
    }

    public void UI_GamePlay()
    {
        if (isThoughtEnabled == true)
        {
            playerThought.enabled = true;
            playerThoughtText.enabled = true;
            isThoughtEnabled = false;
        }

        player.GetComponent<Interaction>().enabled = true;
        playerSprite.GetComponent<Animator>().enabled = true;
        PlayerNGame(true, CursorLockMode.Locked, false, 1f);
        SetUIActive(gameUI);
    }

    public void UI_Pause()
    {
        if (playerThought.enabled == true)
        {
            isThoughtEnabled = true;
            playerThought.enabled = false;
            playerThoughtText.enabled = false;
        }
        buttonSelect.Change("Pause");
        PlayerNGame(false, CursorLockMode.None, true, 0f);
        SetUIActive(pauseUI);
    }

      public void UI_Credits()
    {
        buttonSelect.Change("Credits");
        PlayerNGame(false, CursorLockMode.None, true, 0f);
        playerThought.enabled = false;
        playerThoughtText.enabled = false;
        SetUIActive(creditsUI);
    }

    public void UI_Confirmation()
    {
        buttonSelect.Change("Confirmation");
        SetUIActive(confirmationUI);
    }

    public void UI_GameCompleted()
    {
        buttonSelect.Change("GameComplete");
        PlayerNGame(false, CursorLockMode.None, true, 0f);
        playerThought.enabled = false;
        playerThoughtText.enabled = false;
        SetUIActive(gameCompletedUI);
    }

    public void UI_Options()
    {
        buttonSelect.Change("Options");
        PlayerNGame(false, CursorLockMode.None, true, 0f);
        SetUIActive(optionsUI);
    }

    public void UI_Dialogue()
    {
        buttonSelect.Change("Dialogue");
        PlayerNGame(true, CursorLockMode.None, true, 1f);
        canvGroup.alpha = 0f;
        SetUIActive(dialogueUI);
        StartCoroutine(FadeObject(canvGroup, 0f, 1f));

        player.GetComponent<Interaction>().enabled = false;
        playerSprite.GetComponent<Animator>().enabled = false;
    }

    public void UI_Controls()
    {
        buttonSelect.Change("Controls");
        PlayerNGame(false, CursorLockMode.None, true, 0f);
        SetUIActive(controlsUI);
        KeyboardBindingsActive();
    }

    void SetUIActive(GameObject activeUI)
    {
        menuUI.SetActive(false);
        pauseUI.SetActive(false);
        gameUI.SetActive(false);
        optionsUI.SetActive(false);
        creditsUI.SetActive(false);
        gameCompletedUI.SetActive(false);
        dialogueUI.SetActive(false);
        confirmationUI.SetActive(false);
        controlsUI.SetActive(false);

        activeUI.SetActive(true);
    }

    void PlayerNGame(bool art, CursorLockMode lockMode, bool visible, float time)
    {
        playerSprite.SetActive(art);
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
        Time.timeScale = time;
    }

    public void UpdateItemsUI(ItemSlot[] items)
    {
        for (int i = 0; i < itemUISlots.Length; i++)
        {
            itemUISlots[i].SetItemSlot(items[i]);
        }
    }

    IEnumerator FadeObject(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < fadeSpeed)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / fadeSpeed);

            yield return null;
        }
    }

    public void GamepadBindingsActive()
    {
        keyboardBindings.SetActive(false);
        keyboardButton.enabled = true;
        gamepadBindings.SetActive(true);
        gamepadButton.enabled = false;
        controlsText.text = "Gamepad Controls";
    }

    public void KeyboardBindingsActive()
    {
        gamepadBindings.SetActive(false);
        //gamepadButton.enabled = true;
        keyboardBindings.SetActive(true);
        keyboardButton.enabled = false;
        controlsText.text = "Keyboard Controls";
    }
}