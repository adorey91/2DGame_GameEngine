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
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _triggerCollider;
    private GameObject colliderObject;

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

        if (this.gameObject.GetComponent<SpriteRenderer>() != null && interactType == InteractType.Pickup)
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _triggerCollider = gameObject.GetComponent<CircleCollider2D>();

            Transform colliderTransform = transform.Find("collider");
            colliderObject = colliderTransform.gameObject;
        }


        // if the scene is reloaded this stops an item from respawning back into that scene.
        if (itemToGive != null && itemToGive.isCollected)
            gameObject.SetActive(false);
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
        colliderObject.SetActive(false);
        _spriteRenderer.enabled = false;
        StartCoroutine(ShowInfo(infoMessage, delayTime));
        _inventoryManager.AddItem(itemToGive);
    }

    public void Dialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue, this);

        if (itemToGive != null)
            itemToGive.isCollected = true;
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
}
