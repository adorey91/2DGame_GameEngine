using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestAsset[] quests;
    [SerializeField] private QuestUI questUI;
    private InventoryManager inventoryManager;


    [Header("Quest Hints")]
    [SerializeField] private GameObject[] frogs;
    [SerializeField] private GameObject potionHerb;
    [SerializeField] private GameObject graveyardKey;
    [SerializeField] private GameObject castleKey;


    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        ResetAllQuests();
    }

    #region Quest_Status
    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.InProgress;

        if (quest.name == "The Witch's Help")
            questUI.AddQuestUI(quests[5]);

        questUI.AddQuestUI(quest);

        switch (quest.Name)
        {
            case "The Witch's Help":
                foreach (var frog in frogs)
                {
                    ActivateQuestHints(frog);
                }
                break;
            case "The Potion for the Duck King":
                for (int i = 0; i < quest.prerequisite.questAmountRequired; i++)
                {
                    // inventory manager remove item
                }
                break;
            case "The Graveyard Keeper's Key":
                ActivateQuestHints(graveyardKey);
                break;
            case "The Key to the Duck King's Fortress":
                ActivateQuestHints(castleKey);
                break;
        }
    }

    public void CheckActiveQuest(QuestAsset quest)
    {
        // Check if quest is completed first.
        if (inventoryManager.GetItemQuantity(quest.questItemRequired) == quest.questAmountRequired)
            CompleteQuest(quest);

        // If all quests are completed it will load the Game Complete state when the game is in gameplay only, NOT IN THE CASTLE.
        bool allQuestsCompleted = quests.All(quest => quest.State == QuestAsset.QuestState.Complete);
        if (allQuestsCompleted && SceneManager.GetActiveScene().name == "Gameplay_field")
            Debug.Log("do something here");
        //       gameManager.GameComplete();
    }

    public void CompleteQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Complete;

        questUI.RemoveQuestUI(quest);

        // remove item from inventory
        if (inventoryManager.GetItemQuantity(quest.questItemRequired) == quest.questAmountRequired && quest.name != "The Witch's Help")
        {
            for (int i = 0; i < quest.questAmountRequired; i++)
            {
                inventoryManager.RemoveItem(quest.questItemRequired);
            }
        }

        // add item to inventory if suppose to be given
        if (quest.State == QuestAsset.QuestState.Complete && quest.itemGiven != null)
            inventoryManager.AddItem(quest.itemGiven);
    }

    #endregion

    #region Reset_Activate_Deactivate_Quests
    // Resets all quests
    public void ResetAllQuests()
    {
        if(potionHerb!= null)
            DeactivateQuestHints(potionHerb);
        if(graveyardKey != null)
            DeactivateQuestHints(graveyardKey);
        if(castleKey != null)
            DeactivateQuestHints(castleKey);

        for (int i = 0; i < frogs.Length; i++)
        {
            if (frogs[i] != null)
                DeactivateQuestHints(frogs[i]);
        }
    }

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
    #endregion

    public void FindItems()
    {
        if (graveyardKey == null || frogs == null || potionHerb == null || castleKey == null)
        {
            castleKey = GameObject.Find("Interactable - CastleKey");
            graveyardKey = GameObject.Find("Interactable - GraveyardKey");
            potionHerb = GameObject.Find("Interactable - PickupHerb");
            frogs = GameObject.FindGameObjectsWithTag("Frog");
        }
    }
}
