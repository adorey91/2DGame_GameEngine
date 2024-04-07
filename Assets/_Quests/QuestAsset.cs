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

    [TextArea(2, 10)]
    public string[] InactiveQuestDialogue;
    public string[] ActiveQuestDialogue;
    public string[] CompletedQuestDialogue;

    public enum QuestState
    {
        Inactive,
        Active,
        Completed,
    }
}
