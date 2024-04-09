using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] QuestSlotUI[] uiSlots;

    public void UpdateUI(QuestSlot[] quests)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].SetQuestSlots(quests[i]);
        }
    }
}
