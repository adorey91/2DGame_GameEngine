using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Managers
    private GameManager gameManager;
    private QuestManager questManager;

    // Dialogue Sentences
    private Queue<string> sentences;

    // Interactable Object
    private InteractableObject interactableObject;
    private QuestAsset questAsset;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private TMP_Text dialogueName;
    [SerializeField] private TMP_Text dialogueText;
    private Dialogue personTalking;
    private PlayerController playerController;
    private bool startQuest = false;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.06f;
    private bool canContinueToNextLine = true;
    private Coroutine displayLineCoroutine;
    private bool isAddingRichText;
    private bool NPCtalking = false;
    internal bool skipText;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        questManager = FindObjectOfType<QuestManager>();

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, InteractableObject _interactableObject)
    {
        gameManager.LoadState("Dialogue");
        sentences.Clear();
        NPCtalking = false;
        interactableObject = _interactableObject;

        personTalking = dialogue;
        questAsset = dialogue.assignedQuest;

        if (questAsset != null)
        {
            if (questAsset.prerequisite != null && questAsset.prerequisite.State != QuestAsset.QuestState.Complete)
            {
                sentences.Enqueue("You need to complete a previous task. Come back later.");
                NPCtalking = true;
            }
            else
            {
                startQuest = true;
                questManager.CheckActiveQuest(questAsset);
                NPCtalking = true;
            }
        }

        dialogueName.text = dialogue.characterName;

        ChangeDialogue(dialogue);
    }

    public void ChangeDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        if (questAsset == null || (questAsset.prerequisite != null && questAsset.prerequisite.State != QuestAsset.QuestState.Complete))
        {
            foreach (string sentence in dialogue.NonQuestDialogue)
            {
                sentences.Enqueue(sentence);
            }
        }
        else
        {
            var dialogueLines = questAsset.GetDialogueLines();
            foreach (string sentence in dialogueLines)
            {
                sentences.Enqueue(sentence);
            }
        }
        DisplayNextSentence();
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
            if (skipText || NPCtalking == false)
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
        if (startQuest && questAsset.State == QuestAsset.QuestState.Inactive)
        {
            interactableObject.Info();
            questManager.StartQuest(personTalking.assignedQuest);
            startQuest = false;
        }

        sentences.Clear();
        playerController.noMoving = false;
        gameManager.LoadState("GamePlay");
    }
}