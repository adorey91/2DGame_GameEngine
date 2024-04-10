using UnityEngine;
using System;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameManager _gameManager;
    [Header("UI Objects")]
    public GameObject menuUI;
    public GameObject pauseUI;
    public GameObject gameUI;
    public GameObject optionsUI;
    public GameObject creditsUI;
    public GameObject gameCompletedUI;
    public GameObject dialogueUI;
    public GameObject confirmationUI;

    [Header("Player Settings")]
    public GameObject player;
    public GameObject playerSprite;
    public Image playerThought;
    public TMP_Text playerThoughtText;
    private PlayerController playerController;
    bool isThoughtEnabled;

    [Header("Inventory")]
    [SerializeField] InventorySlotUI[] itemUISlots;

    [Header("DialogueFade")]
    [SerializeField] float fadeSpeed;
    private CanvasGroup canvGroup;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        canvGroup = dialogueUI.GetComponent<CanvasGroup>();
    }

    public void UI_MainMenu()
    {
        PlayerNGame(false, false, CursorLockMode.None, true, 0f);
        playerThought.enabled = false;
        playerThoughtText.enabled = false;
        SetUIActive(menuUI);
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
        PlayerNGame(true, true, CursorLockMode.Locked, false, 1f);
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

        PlayerNGame(false, false, CursorLockMode.None, true, 0f);
        SetUIActive(pauseUI);
    }

      public void UI_Credits()
    {
        PlayerNGame(false, false, CursorLockMode.None, true, 0f);
        playerThought.enabled = false;
        playerThoughtText.enabled = false;
        SetUIActive(creditsUI);
    }

    public void UI_Confirmation()
    {
        SetUIActive(confirmationUI);
    }

    public void UI_GameCompleted()
    {
        PlayerNGame(false, false, CursorLockMode.None, true, 0f);
        playerThought.enabled = false;
        playerThoughtText.enabled = false;
        SetUIActive(gameCompletedUI);
    }

    public void UI_Options()
    {
        PlayerNGame(false, false, CursorLockMode.None, true, 0f);
        SetUIActive(optionsUI);
    }

    public void UI_Dialogue()
    {
        PlayerNGame(true, false, CursorLockMode.None, true, 1f);
        canvGroup.alpha = 0f;
        SetUIActive(dialogueUI);
        StartCoroutine(FadeObject(canvGroup, 0f, 1f));

        player.GetComponent<Interaction>().enabled = false;
        playerSprite.GetComponent<Animator>().enabled = false;
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

        activeUI.SetActive(true);
    }

    void PlayerNGame(bool art, bool controller, CursorLockMode lockMode, bool visible, float time)
    {
        playerSprite.SetActive(art);
        playerController.enabled = controller;
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
}