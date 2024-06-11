using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public QuestAsset[] quests;
    private InventoryManager _inventoryManager;
    private GameManager _gameManager;

    [Header("Hints/QuestNPCs")]
    [SerializeField] private GameObject[] frogs;

    [SerializeField] private GameObject potionHerb;

    [SerializeField] private GameObject graveyardKey;
    [SerializeField] private GameObject castleKey;

    [SerializeField] private bool resetAll;

    //When game is started it looks for the game manager, inventory manager and makes sure that all quests have been reset to Inactive
    void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _gameManager = FindObjectOfType<GameManager>();

        ResetAllQuests();
    }

    public void Update()
    {
        // When this scene is loaded the quest manager needs to find these objects or quests won't work properly.
        if (SceneManager.GetActiveScene().name == "Gameplay_field")
        {
            // Find the game objects if they are null

            if (graveyardKey == null || frogs == null || castleKey == null)
            {
                castleKey = GameObject.Find("Interactable - CastleKey");
                graveyardKey = GameObject.Find("Interactable - GraveyardKey");
                frogs = GameObject.FindGameObjectsWithTag("Frog");
            }

            // Reset all quest hints if needed
            if (resetAll)
            {
                //potionHerb.GetComponent<CircleCollider2D>().enabled = false;
                //potionHerb.GetComponentInChildren<BoxCollider2D>().enabled = false;

                DeactivateQuestHints(graveyardKey);
                DeactivateQuestHints(castleKey);

                for (int i = 0; i < frogs.Length; i++)
                {
                    DeactivateQuestHints(frogs[i]);
                }

                resetAll = false;
            }
        }

        // If all quests are completed it will load the Game Complete state when the game is in gameplay only, NOT IN THE CASTLE.
        bool allQuestsCompleted = quests.All(quest => quest.State == QuestAsset.QuestState.Completed);
        if (allQuestsCompleted && SceneManager.GetActiveScene().name == "Gameplay_field" && !_gameManager.isPaused)
        {
            _gameManager.GameCompleted();
        }
    }

    // Resets all quests
    public void ResetAllQuests()
    {
        foreach (QuestAsset quest in quests) // sets all quests to inactive on start
        {
            quest.State = QuestAsset.QuestState.Inactive;
        }
        resetAll = true;
    }


    // This starts the quest and will deactivate certain hints or activate NPC's
    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.InProgress;

        switch (quest.name)
        {
            case "The Witch's Help":
                for (int i = 0; i < frogs.Length; i++)
                {
                    ActivateQuestHints(frogs[i]);
                }
                break;
            case "The Potion for the Duck King":
                for (int i = 0; i < quest.prerequisite.QuestAmountReq; i++)
                {
                    _inventoryManager.RemoveItem(quest.prerequisite.QuestItemRequired);
                }
                //potionHerb.GetComponent<CircleCollider2D>().enabled = false;
                //potionHerb.GetComponentInChildren<BoxCollider2D>().enabled = false;
                break;
            case "The Graveyard Keeper's Key":
                ActivateQuestHints(graveyardKey);
                break;
            case "The Key to the Duck King's Fortress":
                ActivateQuestHints(castleKey);
                break;
        }
    }

    // Checks if you have the Collectables required to finish the active quest
    public void CheckActiveQuest(QuestAsset quest)
    {
        if (quest.name != "The Witch's Help")
        {
            if (_inventoryManager.GetItemQuantity(quest.QuestItemRequired) == quest.QuestAmountReq)
                CompleteQuest(quest);
        }
    }


    // When the quest is complete, it will run this. if an item is required it will remove the item from the player's inventory
    // Or it will place an item in the player's inventory.
    public void CompleteQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Completed;
        if (_inventoryManager.GetItemQuantity(quest.QuestItemRequired) == quest.QuestAmountReq)
        {
            for (int i = 0; i < quest.QuestAmountReq; i++)
                _inventoryManager.RemoveItem(quest.QuestItemRequired);
        }

        if (quest.State == QuestAsset.QuestState.Completed && quest.GivenAfterCompleted != null)
            _inventoryManager.AddItem(quest.GivenAfterCompleted);
    }

    // used to activate hints. You can put in a number of Game objects that you want activated or just one.
    public void ActivateQuestHints(params GameObject[] hints)
    {
        foreach (GameObject hint in hints)
        {
            hint.GetComponent<CircleCollider2D>().enabled = true;
            hint.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }
    }

    // Deactivates quest hints. Only one is needed or I'd use the same function as above.
    public void DeactivateQuestHints(params GameObject[] hints)
    {
        foreach (GameObject hint in hints)
        {
            hint.GetComponent<SpriteRenderer>().enabled = false;
            hint.GetComponent<CircleCollider2D>().enabled = false;
            hint.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
    }
}
