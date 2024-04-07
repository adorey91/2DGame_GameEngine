using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string characterName;

    [TextArea(2, 10)]
    public string[] NonQuestDialogue;

    public string continueDialogue;
    public string endDialogue;
    public QuestAsset AssignedQuest;
}