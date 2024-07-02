using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // Managers
    [Header("Managers")]
    private GameManager gameManager;
    private QuestManager questManager;

    // Dialogue Sentences
    private Queue<string> sentences;

    // Interactable Object
    private InteractableObject interactableObject;
    private QuestAsset questAsset;

    // Dialogue UI
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private TMP_Text dialogueName;
    [SerializeField] private TMP_Text dialogueText;
    private Dialogue personTalking;
    private bool startQuest = false;
    private bool endQuest = false;

    // Typewriter Settings
    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.06f;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = true;
    private bool isAddringRichText;
    internal bool skipText;

    // Player
    [Header("Player")]
    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        questManager = FindObjectOfType<QuestManager>();

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, InteractableObject _interactableObject)
    {
        gameManager.LoadState("Dialogue");
        playerInput.actions.FindAction("Move").Disable();
        sentences.Clear();
        interactableObject = _interactableObject;

        personTalking = dialogue;
        questAsset = dialogue.assignedQuest;

        dialogueName.text = dialogue.characterName;


        if (questAsset == null)
        {
            NonQuestDialogue(dialogue);
            return;
        }

        if(questAsset.State != QuestAsset.QuestState.Complete)
        {
            if(questManager.CheckActiveQuest(questAsset))
                endQuest = true;
        }

        if (questAsset.State == QuestAsset.QuestState.Complete)
        {
            if (endQuest)
            {
                QuestDialogue();
                endQuest = false;
            }
            else
            {
                NonQuestDialogue(dialogue);
            }
        }
        else if (questAsset.State == QuestAsset.QuestState.Inactive)
        {
            if (questAsset.prerequisite != null && questAsset.prerequisite.State != QuestAsset.QuestState.Complete)
            {
                sentences.Enqueue("You need to complete a previous task. Come back later.");
                DisplayNextSentence();
            }
            else
            {
                startQuest = true;
                QuestDialogue();
            }
        }
        else
        {
            QuestDialogue();
        }
    }

    private void QuestDialogue()
    {
        sentences.Clear();

        var dialogueLines = questAsset.GetDialogueLines();
        foreach (string sentence in dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    private void NonQuestDialogue(Dialogue dialogue)
    {
        foreach (string sentence in dialogue.NonQuestDialogue)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if(canContinueToNextLine)
        {
            if(sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            if(displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            string sentence = sentences.Dequeue();
            skipText = false;
            displayLineCoroutine = StartCoroutine(DisplayLine(sentence));
        }
    }

    private void EndDialogue()
    {
        sentences.Clear();
        playerInput.actions.FindAction("Move").Enable();
        gameManager.LoadState("Gameplay");
        
        if(startQuest)
        {
            StartCoroutine(interactableObject.ShowInfo("New Quest Started", 1f));
            questManager.StartQuest(personTalking.assignedQuest);
            startQuest = false;
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        dialogueButton.SetActive(false);

        canContinueToNextLine = false;
        isAddringRichText = false;
        skipText = false;

        List<char> richTextBuffer = new();

        for(int i = 0; i < line.Length; i++)
        {
            char letter = line[i];

            if(letter == '<')
                isAddringRichText = true;

            if (isAddringRichText)
            {
                richTextBuffer.Add(letter);

                if (letter == '>')
                {
                    isAddringRichText = false;
                    dialogueText.text += new string(richTextBuffer.ToArray());
                    richTextBuffer.Clear();
                }
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            if(skipText)
            {
                dialogueText.text = line;
                break;
            }

        }

        canContinueToNextLine = true;
        dialogueButton.SetActive(true);
    }
}