using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private PlayerController controller;
    [SerializeField] private InventoryUIManager inventoryUIManager;
    [SerializeField] private ControlBindingsManager controlBindingsManager;
    [SerializeField] private UIButtons uiButtons;

    [Header("UI GameStates")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject gameCompletedUI;

    [Header("UI for Non-states")]
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject confirmationUI;
    [SerializeField] private GameObject controlsUI;

    [SerializeField] private CanvasGroup dialogueCanvasGroup;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        dialogueCanvasGroup = dialogueUI.GetComponent<CanvasGroup>();
    }

    // State UI
    public void UI_MainMenu() => SetActiveUI(mainMenuUI);
    public void UI_Gameplay() => SetActiveUI(gameplayUI);
    public void UI_Pause() => SetActiveUI(pauseUI);
    public void UI_Options() => SetActiveUI(optionsUI);
    public void UI_GameCompleted()
    {
        SetActiveUI(gameCompletedUI);
        uiButtons.QuitCreditsButton(true);
        playerUI.TogglePlayer(false);
    }

    // Non state UI
    public void UI_Credits()
    {
        SetActiveUI(creditsUI);
        uiButtons.QuitCreditsButton(false);
    }
    public void UI_Confirmation(string where)
    {
        SetActiveUI(confirmationUI);
        GetComponentInChildren<UIButtons>().SetConfirmation(where);
    }
    public void UI_Controls() => SetActiveUI(controlsUI);

    public void UI_Dialogue()
    {
        dialogueCanvasGroup.alpha = 0f;
        SetActiveUI(dialogueUI);
        StartCoroutine(FadeObject(dialogueCanvasGroup, 0f, 1f));
    }

    private void SetActiveUI(GameObject activeUI)
    {
        mainMenuUI.SetActive(false);
        pauseUI.SetActive(false);
        gameplayUI.SetActive(false);
        optionsUI.SetActive(false);
        gameCompletedUI.SetActive(false);
        dialogueUI.SetActive(false);
        creditsUI.SetActive(false);
        confirmationUI.SetActive(false);
        controlsUI.SetActive(false);

        activeUI.SetActive(true);
    }

    private IEnumerator FadeObject(CanvasGroup canvasGroup, float start, float end)
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