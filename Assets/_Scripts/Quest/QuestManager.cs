using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestAsset[] quests;
    [SerializeField] private QuestUI questUI;
    [SerializeField] private PlayerUI playerUI;
    private InventoryManager inventoryManager;
    LevelManager levelManager;

    public bool resetAll = false;


    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        ResetAllQuests();
    }

    private void Update()
    {
        // Used for debugging purposes
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    foreach (QuestAsset quest in quests)
        //        quest.State = QuestAsset.QuestState.Complete;
        //}

        AllQuestsComplete();
    }

    #region Quest_Status
    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.InProgress;
   
        if (quest.NameOfQuest == "The Witch's Help")
        {
            quests[1].State = QuestAsset.QuestState.InProgress;
            questUI.AddQuestUI(quests[1]);
        }

        questUI.AddQuestUI(quest);

        switch (quest.NameOfQuest)
        {
            case "The Witch's Help":
                questUI.ActivatePickup("Frog");
                break;
            case "Herbs for the Potion":
                for (int i = 0; i < quest.prerequisite.questAmountRequired; i++)
                    inventoryManager.RemoveItem(quest.prerequisite.questItemRequired);

                questUI.ActivatePickup("Herb");
                break;
            case "The Graveyard Keeper's Key":
                questUI.ActivatePickup("GraveyardKey");
                break;
            case "The Key to the Duck King's Fortress":
                questUI.ActivatePickup("CastleKey");
                break;
        }
    }

    public bool CheckActiveQuest(QuestAsset quest)
    {
        // Check if quest is completed first.
        if (inventoryManager.GetItemQuantity(quest.questItemRequired) == quest.questAmountRequired)
        {
            CompleteQuest(quest);
            return true;
        }
        else
            return false;
    }

    private void AllQuestsComplete()
    {
        // If all quests are completed it will load the Game Complete state when the game is in gameplay only, NOT IN THE CASTLE.
        bool allQuestsCompleted = quests.All(quest => quest.State == QuestAsset.QuestState.Complete);
        if (allQuestsCompleted && SceneManager.GetActiveScene().name == "Gameplay_field")
            levelManager.LoadScene("GameEnd");
    }

    public void CompleteQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Complete;

        questUI.RemoveQuestUI(quest);

        // remove item from inventory
        if (inventoryManager.GetItemQuantity(quest.questItemRequired) == quest.questAmountRequired && quest.NameOfQuest != "The Witch's Help")
        {
            for (int i = 0; i < quest.questAmountRequired; i++)
                inventoryManager.RemoveItem(quest.questItemRequired);
        }

        if (quest.NameOfQuest == "Cure The Duck King")
            questUI.ChangeKing();

        // add item to inventory if suppose to be given
        if (quest.State == QuestAsset.QuestState.Complete && quest.itemGiven != null)
        {
            inventoryManager.AddItem(quest.itemGiven);
        }
    }

    #endregion

    // Resets all quests
    public void ResetAllQuests()
    {
        foreach (QuestAsset quest in quests)
            quest.State = QuestAsset.QuestState.Inactive;
    }
}
