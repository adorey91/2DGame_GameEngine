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
    private GameObject[] questhints;
    
    // This is being used so that I can change the quest dialogue without having to find the character the quest is tied to.
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

    // This returns the dialogue lines to the dialogueManager depending on the quest state
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

    public void ActivateQuestHints(GameObject hint1, GameObject hint2)
    {
        questhints[0] = hint1;
        questhints[1] = hint2;

        for(int i = 0; i < questhints.Length; i++)
        {
            questhints[i].GetComponent<SpriteRenderer>().enabled = true;
            questhints[i].GetComponent<CircleCollider2D>().enabled = true;
            questhints[i].GetComponentInChildren<BoxCollider2D>().enabled = true;
        }
    }

    public void DeactivateQuestHints(GameObject hint)
    {

        hint.GetComponent<SpriteRenderer>().enabled = false;
        hint.GetComponent<CircleCollider2D>().enabled = false;
        hint.GetComponentInChildren<BoxCollider2D>().enabled = false;
    }
}
