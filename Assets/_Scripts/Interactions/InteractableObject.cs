using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    public enum Interactable { None, Info, Pickup, Dialogue, }
    public Interactable interactableType;


    [Header("Info Objects")]
    [SerializeField] private string infoMessage;
    [SerializeField] private float delayTime;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private SpriteRenderer thoughtBubble;

    [Header("Pickup Objects")]
    [SerializeField] private ItemData itemGivenToPlayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D triggerCollider;
    [SerializeField] private GameObject colliderObject;

    [Header("Dialogue")]
    public Dialogue dialogue;


    // used for information interactions. All will show up in a thought bubble.
    public void Info()
    {
        StartCoroutine(ShowInfo(infoMessage, delayTime));
    }

    // will turn off all colliders and the currentSprite renderer
    public void Pickup()
    {

        gameObject.GetComponent<AudioSource>().Play();
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;
        colliderObject.SetActive(false);
        StartCoroutine(ShowInfo(infoMessage, delayTime));
        //inventory manager add item
        //inventoryManager.AddItem(itemToGive);
        SetCollected("true");
    }

    public void Dialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue, this);
    }

    private IEnumerator ShowInfo(string message, float delay)
    {
        thoughtBubble.enabled = true;
        infoText.enabled = true;
        infoText.text = message;

        yield return new WaitForSeconds(delay);

        infoText.text = null;
        thoughtBubble.enabled = false;
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