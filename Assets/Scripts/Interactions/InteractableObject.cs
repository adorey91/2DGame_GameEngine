using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    ItemManager _itemManager;

    [Header("Pickup Settings")]
    [SerializeField] private ItemData itemToGive;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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
        _itemManager = FindObjectOfType<ItemManager>();

        infoUI = GameObject.Find("InfoUI");
        infoText = GameObject.Find("InfoText").GetComponent<TMP_Text>();
        bubble = GameObject.Find("Bubble").GetComponent<Image>();
       
        bubble.enabled = false;
        infoText.text = null;

        //if (itemToGive != null && _itemManager.IsItemCollected(itemToGive))
        if (itemToGive != null && itemToGive.isCollected)
            gameObject.SetActive(false);
    }



    public void Info()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
    }

    public void Pickup()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
        _inventoryManager.AddItem(itemToGive);
        //_itemManager.CollectItem(itemToGive);
    }

    public void Dialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue, this);
    }

    IEnumerator ShowInfo(string message, float delay)
    {
        bubble.enabled = true;
        infoText.text = message;
        yield return new WaitForSeconds(delay);

        infoText.text = null;
        bubble.enabled = false;

        if (interactType == InteractType.Pickup)
            this.gameObject.SetActive(false);
    }
}
