using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestAsset[] quests;
    private InventoryManager _inventoryManager;
    [SerializeField] Doors graveyardDoor;

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
        if (graveyardDoor == null)
        {
            graveyardDoor = FindAnyObjectByType<Doors>();
        }
    }

    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Active;

    }

    public void CheckActiveQuest(QuestAsset quest)
    {
        if(_inventoryManager.HasItem(quest.QuestItemRequired))
            CompleteQuest(quest);
    }

    public void CompleteQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Completed;
        if(_inventoryManager.HasItem(quest.QuestItemRequired))
            _inventoryManager.RemoveItem(quest.QuestItemRequired);

        if (quest.name == "GetGraveyardKey" && quest.State == QuestAsset.QuestState.Completed)
        {
            graveyardDoor.OpenDoor();
        }
    }
}
