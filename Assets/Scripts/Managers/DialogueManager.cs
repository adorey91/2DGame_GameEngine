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
    private PlayerController playerController;
    bool startQuest = false;

    [Header("Typewriter Settings")]
    [SerializeField] float typingSpeed = 0.06f;
    private bool canContinueToNextLine = true;
    private Coroutine displayLineCoroutine;
    private bool isAddingRichText;
    bool NPCTalking = false;
    internal bool skipText;

    public void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _questManager = FindObjectOfType<QuestManager>();
        playerController = FindObjectOfType<PlayerController>();
        sentences = new Queue<string>();
    }


    // Starts dialogue, clears the previous sentences
    public void StartDialogue(Dialogue dialogue, InteractableObject interactableObject)
    {
        playerController.noMoving = true;
        _gameManager.LoadState("Dialogue");
        sentences.Clear();
        NPCTalking = false;
        _interactableObject = interactableObject;

        personTalking = dialogue;
        _questAsset = dialogue.AssignedQuest;

        if (_questAsset != null)
        {
            // Check if prerequisites are completed
            if (_questAsset.prerequisite != null && _questAsset.prerequisite.State != QuestAsset.QuestState.Completed)
            {
                // Enqueue prerequisite dialogue
                sentences.Enqueue("You need to complete a previous task. Come back later.");
                NPCTalking = true;
            }
            else
            {
                startQuest = true;
                _questManager.CheckActiveQuest(_questAsset);
                NPCTalking = true;
            }
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
            skipText = false;
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
            if ((skipText || NPCTalking == false) && _gameManager.isPaused == false)
            {
                dialogueText.text = line;
                skipText = false;
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
        if (startQuest && _questAsset.State == QuestAsset.QuestState.Inactive)
        {
            _interactableObject.Info();
            _questManager.StartQuest(personTalking.AssignedQuest);
            startQuest = false; 
        }

        sentences.Clear();
        playerController.noMoving = false;
        _gameManager.LoadState("GamePlay");
    }

   public void ChangeDialogue(Dialogue dialogue)
{
    sentences.Clear();

    if (_questAsset == null || (_questAsset.prerequisite != null && _questAsset.prerequisite.State != QuestAsset.QuestState.Completed))
    {
        // Add non-quest related dialogue or prerequisite dialogue
        foreach (string sentence in dialogue.NonQuestDialogue)
        {
            sentences.Enqueue(sentence);
        }
    }
    else
    {
        // Add quest-related dialogue
        var dialogueLines = _questAsset.GetDialogueLines();
        foreach (string sentence in dialogueLines)
        {
            sentences.Enqueue(sentence);
        }
    }
    DisplayNextSentence();
}
}