using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public QuestAsset[] quests;
    private InventoryManager _inventoryManager;
    private GameManager _gameManager;
    private GameObject graveyardDoorObject;
    private GameObject darkCastleDoorObject;
    [SerializeField] Doors graveyardDoor;
    [SerializeField] Doors darkCastleDoor;
    [SerializeField] GameObject noteFromWitch;

    bool activateNoteFromWitch = false;

    void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _gameManager = FindObjectOfType<GameManager>();

        ResetAllQuests();
    }

    public void Update()
    {
        // when it reloads into this scene it needs to find these objects in order for the quests to work properly.
        if (SceneManager.GetActiveScene().name == "Gameplay_field")
        {
            if (graveyardDoorObject == null)
            {
                graveyardDoorObject = GameObject.Find("GraveyardDoors1");
                graveyardDoor = graveyardDoorObject.GetComponent<Doors>();
            }
            else if (darkCastleDoorObject == null)
            {
                darkCastleDoorObject = GameObject.Find("DarkCastleDoors1");
                darkCastleDoor = darkCastleDoorObject.GetComponent<Doors>();
            }
            else if(noteFromWitch == null)
            {
                noteFromWitch = GameObject.Find("Interactable - NoteFromWitch");
            }
            else if(noteFromWitch != null && activateNoteFromWitch == true)
            {
                noteFromWitch.GetComponent<SpriteRenderer>().enabled = true;
                noteFromWitch.GetComponent<CircleCollider2D>().enabled = true;
                noteFromWitch.GetComponentInChildren<BoxCollider2D>().enabled = true;
            }
        }

        // If all quests are completed it will load the Game Complete state when the game is in gameplay only.
        bool allQuestsCompleted = quests.All(quest => quest.State == QuestAsset.QuestState.Completed);
        if(allQuestsCompleted && _gameManager.gameState == GameManager.GameState.Gameplay)
        {
            Debug.Log("All quests completed");
            _gameManager.LoadState("GameComplete");
        }
    }

    public void ResetAllQuests()
    {
        foreach (QuestAsset quest in quests) // sets all quests to inactive on start
        {
            quest.State = QuestAsset.QuestState.Inactive;
        }
    }


    // This starts the quest
    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.InProgress;

        if (quest.name == "HelpTheDuckKing")
            activateNoteFromWitch = true;
    }
   
    // Checks if you have the items required to finish the active quest
    public void CheckActiveQuest(QuestAsset quest)
    {
        if (_inventoryManager.GetItemQuantity(quest.QuestItemRequired) == quest.QuestAmountReq)
            CompleteQuest(quest);
    }

    
    // If you complete a quest, this can happen. 
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

        if (quest.name == "GetGraveyardKey" && quest.State == QuestAsset.QuestState.Completed)
            graveyardDoor.OpenDoor();
        if (quest.name == "GetDarkCastleKey" && quest.State == QuestAsset.QuestState.Completed)
            darkCastleDoor.OpenDoor();
    }
}
