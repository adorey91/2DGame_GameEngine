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

    [Header("Pickup Settings")]
    [SerializeField] private ItemData itemToGive;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Information")]
    public string infoMessage;
    public float delayTime;
    public GameObject infoUI;
    [SerializeField] TMP_Text infoText;
    [SerializeField] Image bubble;
    [Header("Dialogue Settings")]
    public Dialogue dialogue;


    public void Awake()
    {
        _inventoryManager = FindAnyObjectByType<InventoryManager>();

        infoUI = GameObject.Find("InfoUI");
        infoText = GameObject.Find("InfoText").GetComponent<TMP_Text>();
        bubble = GameObject.Find("Bubble").GetComponent<Image>();
        bubble.enabled = false;
        infoText.text = null;
    }

    private void Start()
    {
        if (itemToGive != null)
        {
            if (itemToGive.collected == true)
                Destroy(this.gameObject);
        }
    }


    public void Info()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
    }

    public void Pickup()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
        _inventoryManager.AddItem(itemToGive);
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
