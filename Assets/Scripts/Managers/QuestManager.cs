using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public QuestAsset[] quests;
    private InventoryManager _inventoryManager;
    private GameManager _gameManager;

    [Header("Hints/QuestNPCs")]
    [SerializeField] private GameObject noteFromWitch;
    [SerializeField] private GameObject noteFromClifford;
    [SerializeField] private GameObject witch;
    [SerializeField] private GameObject[] frogs;
    [SerializeField] private GameObject graveyardKey;

    private bool activateNoteFromWitch = false;
    private bool deactivateNoteFromClifford = false;
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

            if (noteFromWitch == null || witch == null || noteFromClifford == null || graveyardKey == null || frogs == null)
            {
                noteFromWitch = GameObject.Find("Interactable - NoteFromWitch");
                witch = GameObject.Find("Interactable - Dialogue - Witch");
                noteFromClifford = GameObject.Find("Interactable - NoteFromClifford");
                graveyardKey = GameObject.Find("Interactable - Pickupkey");
                frogs = GameObject.FindGameObjectsWithTag("Frog");
            }

            // Reset all quest hints if needed
            if (resetAll)
            {
                graveyardKey.GetComponent<CircleCollider2D>().enabled = false;
                graveyardKey.GetComponentInChildren<BoxCollider2D>().enabled = false;

                for(int i = 0; i < frogs.Length; i++)
                {
                    frogs[i].GetComponent<CircleCollider2D>().enabled = false;
                    frogs[i].GetComponentInChildren<BoxCollider2D>().enabled = false;
                }

                activateNoteFromWitch = false;
                deactivateNoteFromClifford = false;
                resetAll = false;
            }

            // Activate/deactivate Clifford quest hints based on flags
            if (deactivateNoteFromClifford)
                DeactivateQuestHints(noteFromClifford);
            else
                ActivateQuestHints(noteFromClifford);

            // Activate/deactivate Witch quest hints based on flags
            if (activateNoteFromWitch)
                ActivateQuestHints(noteFromWitch, witch);
            else
                DeactivateQuestHints(noteFromWitch);

        }

        // If all quests are completed it will load the Game Complete state when the game is in gameplay only, NOT IN THE CASTLE.
        bool allQuestsCompleted = quests.All(quest => quest.State == QuestAsset.QuestState.Completed);
        if(allQuestsCompleted && SceneManager.GetActiveScene().name == "Gameplay_field" && !_gameManager.isPaused)
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

        if (quest.name == "GetCliffordFood")
            deactivateNoteFromClifford = true;
        else if(quest.name == "GetGraveyardKey")
        {
            graveyardKey.GetComponent<CircleCollider2D>().enabled = true;
            graveyardKey.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }
        else if (quest.name == "HelpTheDuckKing")
            activateNoteFromWitch = true;
        else if (quest.name == "BringTheWitchFrogs")
        {
            for (int i = 0; i < frogs.Length; i++)
            {
                frogs[i].GetComponent<CircleCollider2D>().enabled = true;
                frogs[i].GetComponentInChildren<BoxCollider2D>().enabled = true;
            }
            activateNoteFromWitch = false;
        }
    }
   
    // Checks if you have the Collectables required to finish the active quest
    public void CheckActiveQuest(QuestAsset quest)
    {
        if (_inventoryManager.GetItemQuantity(quest.QuestItemRequired) == quest.QuestAmountReq)
            CompleteQuest(quest);
    }

    
    // When the quest is complete, it will run this. if an item is required it will remove the item from the player's inventory
    // Or it will place an item in the player's inventory.
    public void CompleteQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Completed;
        if (_inventoryManager.GetItemQuantity(quest.QuestItemRequired) == quest.QuestAmountReq)
        {
            for (int i = 0; i < quest.QuestAmountReq; i++)
            {
                _inventoryManager.RemoveItem(quest.QuestItemRequired);
            }
        }

        if(quest.State == QuestAsset.QuestState.Completed && quest.GivenAfterCompleted != null)
            _inventoryManager.AddItem(quest.GivenAfterCompleted);
    }

    // used to activate hints. You can put in a number of Game objects that you want activated or just one.
    public void ActivateQuestHints(params GameObject[] hints)
    {
        foreach (GameObject hint in hints)
        {
            hint.GetComponent<SpriteRenderer>().enabled = true;
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
