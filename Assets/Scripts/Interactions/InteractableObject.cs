using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public enum InteractType
    {
        Info,
        Pickup,
        Dialogue,
    }

    public InteractType interactType;
    InventoryManager _inventoryManager;

    [Header("Pickup Settings")]
    [SerializeField] private ItemData itemToGive;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CircleCollider2D _triggerCollider;
    [SerializeField] private GameObject colliderObject;
   
    [Header("Information")]
    public string infoMessage;
    public float delayTime;
    public GameObject infoUI;
    [SerializeField] TMP_Text infoText; // player information text
    [SerializeField] Image bubble; //player information bubble
    
    [Header("Dialogue Settings")]
    public Dialogue dialogue;

    public void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();

        infoUI = GameObject.Find("InfoUI");
        infoText = GameObject.Find("InfoText").GetComponent<TMP_Text>();
        bubble = GameObject.Find("Bubble").GetComponent<Image>();

        bubble.enabled = false;
        infoText.text = null;
    }

    // used for information interactions. All will show up in a thought bubble.
    public void Info()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
    }

    // will turn off all colliders and the currentSprite renderer
    public void Pickup()
    {

        gameObject.GetComponent<AudioSource>().Play();
        _triggerCollider.enabled = false;
        _spriteRenderer.enabled = false;
        colliderObject.SetActive(false);
        StartCoroutine(ShowInfo(infoMessage, delayTime));
        _inventoryManager.AddItem(itemToGive);
        SetCollected("true");
    }

    public void Dialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue, this);
    }


    IEnumerator ShowInfo(string message, float delay)
    {
        bubble.enabled = true;
        infoText.enabled = true;
        infoText.text = message;

        yield return new WaitForSeconds(delay);

        infoText.text = null;
        bubble.enabled = false;
    }

    public void SetCollected(string Collected)
    {
        string uniqueKey = SceneManager.GetActiveScene().name + "." + gameObject.name;
        PlayerPrefs.SetString("collectables." + uniqueKey, Collected);
    }

    public bool IsCollected()
    {
        string sceneAndName = SceneManager.GetActiveScene().name + "." + gameObject.name;
        string collectedStatus = PlayerPrefs.GetString("collectables." + sceneAndName, "false");
        return collectedStatus == "true";
    }
}
