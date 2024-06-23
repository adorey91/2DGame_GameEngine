using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    // UI for quest
    [Header("Quest List UI")]
    [SerializeField] private GameObject questNameObject;
    [SerializeField] private GameObject questHolder;
    [SerializeField] private TMP_Text questNameText;
    private Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>();

    // UI for quest hints
    [Header("Quest Hints")]
    [SerializeField] private GameObject[] frogs;
    [SerializeField] private GameObject[] potionHerb;
    [SerializeField] private GameObject graveyardKey;
    [SerializeField] private GameObject castleKey;

    [Header("Quest Completed Things")]
    [SerializeField] private GameObject castleDoor;
    [SerializeField] private GameObject graveyardDoor;
    [SerializeField] private GameObject duckKing;

    public enum QuestPickupType { Herb, Frog, CastleKey, GraveyardKey }

    private bool firstLoad;

    private void Start()
    {
        firstLoad = true;
    }

    // Removes quest from UI
    public void RemoveQuestUI(QuestAsset quest)
    {
        if (activeQuests.TryGetValue(quest.NameOfQuest, out GameObject foundQuest))
        {
            Destroy(foundQuest);
            activeQuests.Remove(quest.NameOfQuest);
        }
    }

    // Adds quest to UI
    public void AddQuestUI(QuestAsset quest)
    {
        // Instantiate the quest UI element
        GameObject questObject = Instantiate(questNameObject, questHolder.transform);
        questNameText = questObject.GetComponent<TextMeshProUGUI>();

        if (questNameText != null)
        {
            questNameText.text = " - " + quest.NameOfQuest;
            activeQuests.Add(quest.NameOfQuest, questObject);
        }
        else
            Debug.Log("TextMeshProUGUI component not found in prefab");

    }

    public void ActivatePickup(string pickupTypeString)
    {
        if (System.Enum.TryParse(pickupTypeString, true, out QuestPickupType pickupType))
            ActivatePickup(pickupType);
        else
            Debug.LogWarning($"Unknown quest pickup type: {pickupTypeString}");
    }

    public void ActivatePickup(QuestPickupType pickupType)
    {
        switch (pickupType)
        {
            case QuestPickupType.Herb:
                SetQuestPickupState(true, potionHerb);
                break;
            case QuestPickupType.Frog:
                SetQuestPickupState(true, frogs);
                break;
            case QuestPickupType.CastleKey:
                SetQuestPickupState(true, castleKey);
                break;
            case QuestPickupType.GraveyardKey:
                SetQuestPickupState(true, graveyardKey);
                break;
            default:
                Debug.LogWarning("Unknown quest pickup type");
                break;
        }
    }

    private void SetQuestPickupState(bool state, params GameObject[] questPickups)
    {
        foreach (GameObject pickup in questPickups)
        {
            if (pickup != null)
            {
                SetPickupState(pickup, state);
            }
            else
                Debug.LogWarning("Attempted to set state on a null pickup");
        }
    }

    private void SetPickupState(GameObject pickup, bool state)
    {
        if (pickup.GetComponent<SpriteRenderer>())
        {
            var spriteRenderer = pickup.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.enabled = state;
        }

        var colliders = pickup.GetComponentsInChildren<CircleCollider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = state;
        }
    }

    public void ChangeKing()
    {
        SpriteRenderer duckKingSprite;
        duckKingSprite = duckKing.GetComponent<SpriteRenderer>();

        duckKingSprite.color = Color.white;
    }

    public void FindDuckKing()
    {
        if (duckKing == null)
            duckKing = GameObject.Find("DuckKing");
    }

    public void FindItems()
    {
        if (graveyardKey == null)
            graveyardKey = GameObject.Find("Interactable - GraveyardKey");

        if (frogs == null || frogs.Length == 0)
            frogs = GameObject.FindGameObjectsWithTag("Frog");

        if (potionHerb == null || potionHerb.Length == 0)
            potionHerb = GameObject.FindGameObjectsWithTag("Herb");

        if (castleKey == null)
            castleKey = GameObject.Find("Interactable - CastleKey");

        if (firstLoad)
        {
            SetQuestPickupState(false, frogs);
            SetQuestPickupState(false, potionHerb);
            SetQuestPickupState(false, castleKey);
            SetQuestPickupState(false, graveyardKey);
            firstLoad = false;
        }
    }
}
