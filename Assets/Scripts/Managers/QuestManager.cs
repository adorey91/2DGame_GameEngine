using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestAsset[] quests;

    void Start()
    {
        foreach (QuestAsset quest in quests)
        {
            quest.State = QuestAsset.QuestState.Inactive;
        }
    }

    public void StartQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Active;
    }

    public void CompleteQuest(QuestAsset quest)
    {
        quest.State = QuestAsset.QuestState.Completed;
    }
}
