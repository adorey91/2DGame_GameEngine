using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    // UI for quest
    [Header("Quest UI")]
    [SerializeField] private GameObject questNameObject;
    [SerializeField] private GameObject questHolder;

    [SerializeField] private Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>();


    // Removes quest from UI
    public void RemoveQuestUI(QuestAsset quest)
    {
        if (activeQuests.TryGetValue(quest.name, out GameObject foundQuest))
        {
            Object.Destroy(foundQuest);
            activeQuests.Remove(quest.name);
        }
    }

    // Adds quest to UI
    public void AddQuestUI(QuestAsset quest)
    {
        // Instantiate the quest UI element
        GameObject questObject = Instantiate(questNameObject, questHolder.transform);
        TextMeshProUGUI questNameText = questNameObject.GetComponent<TextMeshProUGUI>();

        if (questNameText != null)
            questNameText.text = " - " + quest.name;
        else
            Debug.Log("TextMeshProUGUI component not found in prefab");

        string word = quest.name;
        activeQuests.Add(word, questObject);
    }
}
