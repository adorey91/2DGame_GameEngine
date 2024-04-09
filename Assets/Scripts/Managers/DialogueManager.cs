using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private GameManager _gameManager;
    private QuestManager _questManager;
    private InteractableObject _interactableObject;
    QuestAsset _questAsset;

    [Header("Dialogue UI")]
    public GameObject dialogueButton;
    public TMP_Text dialogueName;
    public TMP_Text dialogueText;
    public TMP_Text dialogueButtonText;
    private string endDialogue;
    Dialogue personTalking;


    [Header("Typewriter Settings")]
    [SerializeField] float typingSpeed = 0.04f;
    private bool canContinueToNextLine = true;
    private Coroutine displayLineCoroutine;
    private bool isAddingRichText;
    bool NPCTalking = false;

    public void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _questManager = FindObjectOfType<QuestManager>();

        sentences = new Queue<string>();
    }

    /// <summary>
    /// Loads dialogue state. 
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue, InteractableObject interactableObject)
    {
        _gameManager.LoadState("Dialogue");
        sentences.Clear();
        NPCTalking = false;
        _interactableObject = interactableObject;

        personTalking = dialogue;
        _questAsset = dialogue.AssignedQuest;

        if (_questAsset != null)
        {
            _questManager.CheckActiveQuest(_questAsset);
            NPCTalking = true;
        }

        dialogueName.text = dialogue.characterName;
        dialogueButtonText.text = dialogue.continueDialogue;
        endDialogue = dialogue.endDialogue;

        ChangeDialogue(dialogue);
    }

    public void DisplayNextSentence()
    {
        if (canContinueToNextLine)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            if (sentences.Count == 1)
                dialogueButtonText.text = endDialogue;

            if (displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            string sentence = sentences.Dequeue();
            displayLineCoroutine = StartCoroutine(DisplayLine(sentence));
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        //empty text
        dialogueText.text = "";
        dialogueButton.SetActive(false);

        canContinueToNextLine = false;

        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKey(KeyCode.Mouse0) || NPCTalking == false)
            {
                dialogueText.text = line;
                break;
            }

            if (letter == '<' || isAddingRichText)
            {
                isAddingRichText = true;
                dialogueText.text += letter;

                if (letter == '>')
                    isAddingRichText = false;
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        canContinueToNextLine = true;
        dialogueButton.SetActive(true);
    }

    void EndDialogue()
    {
        if (_questAsset != null && _questAsset.State == QuestAsset.QuestState.Inactive)
        {
            _interactableObject.Info();
            _questManager.StartQuest(personTalking.AssignedQuest);
        }

        sentences.Clear();

        _gameManager.LoadState("Gameplay");
    }

    public void ChangeDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        if (_questAsset == null)
        {
            // adds non quest related dialogue
            foreach (string sentence in dialogue.NonQuestDialogue)
            {
                sentences.Enqueue(sentence);
            }
        }
        else
        {
            // adds quest related dialogue
            var dialogueLines = _questAsset.GetDialogueLines();
            foreach (string sentence in dialogueLines)
            {
                sentences.Enqueue(sentence);
            }
        }

        DisplayNextSentence();
    }
}