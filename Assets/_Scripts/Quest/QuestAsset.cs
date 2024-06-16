using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/New Quest")]
public class QuestAsset : ScriptableObject
{
    public string Name;
    public QuestState State;

    public QuestAsset prerequisite;
    public ItemData questItemRequired;
    public int questAmountRequired;
    public ItemData itemGiven;


    [TextArea] public string[] InactiveQuestDialogue;
    [TextArea] public string[] ActiveQuestDialogue;
    [TextArea] public string[] CompletedQuestDialogue;

    public enum QuestState { Inactive, InProgress, Complete }

    public string[] GetDialogueLines()
    {
        return State switch
        {
            QuestState.Inactive => InactiveQuestDialogue,
            QuestState.InProgress => ActiveQuestDialogue,
            QuestState.Complete => CompletedQuestDialogue,
            _ => new string[0],// Return empty array if state is unknown
        };
    }
}