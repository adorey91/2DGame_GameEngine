using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool startQuest = false;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.06f;
    private bool canContinueToNextLine = true;
    private Coroutine displayLineCoroutine;
    private bool isAddingRichText;
    internal bool skipText;

    // player
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

        if (questAsset != null)
        {
            if (questAsset.prerequisite != null && questAsset.prerequisite.State != QuestAsset.QuestState.Complete)
            {
                sentences.Enqueue("You need to complete a previous task. Come back later.");
                dialogueName.text = dialogue.characterName;
                DisplayNextSentence(); // Display the prerequisite message directly
            }
            if (questAsset.State == QuestAsset.QuestState.Complete)
            {
                sentences.Enqueue(dialogue.NonQuestDialogue[0]);
                dialogueName.text = dialogue.characterName;
                DisplayNextSentence(); // Display the message directly
            }
            else
            {
                startQuest = true;
                questManager.CheckActiveQuest(questAsset);
                dialogueName.text = dialogue.characterName;
                ChangeDialogue(dialogue);
            }
        }
        else
        {
            dialogueName.text = dialogue.characterName;
            ChangeDialogue(dialogue);
        }
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
        dialogueText.text = "";
        dialogueButton.SetActive(false);

        canContinueToNextLine = false;
        isAddingRichText = false;
        skipText = false;

        List<char> richTextBuffer = new();

        for (int i = 0; i < line.Length; i++)
        {
            char letter = line[i];
            if (letter == '<')
            {
                isAddingRichText = true;
            }

            if (isAddingRichText)
            {
                richTextBuffer.Add(letter);
                if (letter == '>')
                {
                    isAddingRichText = false;
                    dialogueText.text += new string(richTextBuffer.ToArray());
                    richTextBuffer.Clear();
                }
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (skipText)
            {
                dialogueText.text = line;
                break;
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
        playerInput.actions.FindAction("Move").Enable();
        gameManager.LoadState("Gameplay");
    }
}