using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text questName;
    private QuestSlot questSlot;

    public void SetQuestSlots(QuestSlot quest)
    {
        questSlot = quest;

        if(questSlot.name != null)
            questName.text = questSlot.name;
        else
            questName.text = string.Empty;
    }
}
