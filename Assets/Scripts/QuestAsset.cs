using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/New Quest")]
public class QuestAsset : ScriptableObject
{
    public string Name;
    public string Description;
    public QuestState State;
    public ItemData QuestItemRequired;
    public int QuestAmountReq;
    public ItemData GivenAfterCompleted;

    /// <summary>
    /// This is being used so that I can change the quest dialogue without having to find the character the quest is tied to.
    /// </summary>
    [TextArea(2, 10)]
    public string[] InactiveQuestDialogue;
    public string[] ActiveQuestDialogue;
    public string[] CompletedQuestDialogue;


    // holds each state of the quests
    public enum QuestState
    {
        Inactive,
        InProgress,
        Completed,
    }

    public string[] GetDialogueLines()
    {
        switch (State)
        {
            case QuestState.Inactive:
                return InactiveQuestDialogue;
            case QuestState.InProgress:
                return ActiveQuestDialogue;
            case QuestState.Completed:
                return CompletedQuestDialogue;
            default:
                return new string[0]; // Return empty array if state is unknown
        }
    }
}
