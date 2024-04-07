using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public QuestAsset[] quests;
    private InventoryManager _inventoryManager;
    private GameObject graveyardDoorObject;
    private GameObject darkCastleDoorObject;
    [SerializeField] Doors graveyardDoor;
    [SerializeField] Doors darkCastleDoor;

    void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();

        foreach (QuestAsset quest in quests)
        {
            quest.State = QuestAsset.QuestState.Inactive;
        }
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay_field")
        {
            if (graveyardDoorObject == null)
            {
                graveyardDoorObject = GameObject.Find("GraveyardDoors1");
                graveyardDoor = graveyardDoorObject.GetComponent<Doors>();
            }
            if (darkCastleDoorObject == null)
            {
                darkCastleDoorObject = GameObject.Find("DarkCastleDoors1");
                darkCastleDoor = darkCastleDoorObject.GetComponent<Doors>();
            }
        }
    }

    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Active;
    }

    public void CheckActiveQuest(QuestAsset quest)
    {
        if (_inventoryManager.GetItemQuantity(quest.QuestItemRequired) == quest.QuestAmountReq)
            CompleteQuest(quest);
    }

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
