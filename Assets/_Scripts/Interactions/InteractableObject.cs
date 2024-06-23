using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    public enum Interactable { None, Info, Pickup, Dialogue, }
    public Interactable interactableType;

    [Header("Managers")]
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private InteractableObjectUI interactableObjectUI;

    [Header("Info Objects")]
    [SerializeField] private string infoMessage;
    [SerializeField] private float delayTime;
    [SerializeField] private TMP_Text infoText;

    [Header("Pickup Objects")]
    [SerializeField] private ItemData itemGivenToPlayer;
   
    [Header("Dialogue")]
    public Dialogue dialogue;

    public void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        interactableObjectUI = GetComponent<InteractableObjectUI>();
        infoText = GameObject.Find("InfoText").GetComponent<TMP_Text>();
    }

    // used for information interactions. All will show up in a thought bubble.
    public void Info()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
    }

    // will turn off all colliders and the currentSprite renderer
    public void Pickup()
    {
        interactableObjectUI.ToggleInteractable(false);
        FindObjectOfType<PlayerUI>().TogglePlayerInteract(false);
        StartCoroutine(ShowInfo(infoMessage, delayTime));
        //inventory manager add item
        inventoryManager.AddItem(itemGivenToPlayer);
        SetCollected("true");
    }

    public void Dialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue, this);
    }

    private IEnumerator ShowInfo(string message, float delay)
    {
        playerUI = FindAnyObjectByType<PlayerUI>();
        infoText = GameObject.Find("InfoText").GetComponent<TMP_Text>();

        playerUI.TogglePlayerThoughts(true, message);
        infoText.enabled = true;
        infoText.text = message;

        yield return new WaitForSeconds(delay);

        playerUI.TogglePlayerThoughts(false, null);
    }

    public void SetCollected(string collected)
    {
        string uniqueKey = SceneManager.GetActiveScene().name + "." + gameObject.name;
        PlayerPrefs.SetString("collectables." + uniqueKey, collected);
    }

    public bool IsCollected()
    {
        string sceneAndName = SceneManager.GetActiveScene().name + "." + gameObject.name;
        string collectedStatus = PlayerPrefs.GetString("collectables." + sceneAndName, "false");
        return collectedStatus == "true";
    }
}