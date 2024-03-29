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
    private InventoryManager _inventoryManager;
    QuestAsset _questAsset;

    [Header("Dialogue UI")]
    public GameObject dialogueUI;
    public GameObject dialogueButton;
    public TMP_Text dialogueName;
    public TMP_Text dialogueText;
    public TMP_Text dialogueButtonText;
    Dialogue personTalking;

    [Header("Dialogue Fade")]
    [SerializeField] float fadeSpeed;
    private string endDialogue;
    private bool isFaded = true;
    CanvasGroup canvGroup;

    [Header("Typewriter Settings")]
    [SerializeField] float typingSpeed = 0.04f;
    private bool canContinueToNextLine = true;
    private Coroutine displayLineCoroutine;
    private bool isAddingRichText;

    public void Start()
    {
        canvGroup = dialogueUI.GetComponent<CanvasGroup>();
        _gameManager = FindObjectOfType<GameManager>();
        _questManager = FindObjectOfType<QuestManager>();
        sentences = new Queue<string>();
        dialogueUI.SetActive(false);
    }


    public void StartDialogue(Dialogue dialogue)
    {
        _gameManager.LoadState("Dialogue");
        sentences.Clear();
        dialogueUI.SetActive(true);
        StartCoroutine(FadeObject(canvGroup, canvGroup.alpha, isFaded ? 1 : 0));
        isFaded = !isFaded;

        personTalking = dialogue;
        _questAsset = dialogue.AssignedQuest;

        _questManager.CheckActiveQuest(_questAsset);

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
            //if the submit button is pressed
            if (Input.GetKeyDown(_gameManager.interactKey))
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
            _questManager.StartQuest(personTalking.AssignedQuest);

        StartCoroutine(FadeObject(canvGroup, canvGroup.alpha, isFaded ? 1 : 0));
        isFaded = !isFaded;
        sentences.Clear();
        dialogueUI.SetActive(false);

        _gameManager.LoadState("Gameplay");
    }

    public void ChangeDialogue(Dialogue dialogue)
    {
        


        if(_questAsset == null)
        {
            foreach (string sentence in dialogue.NonQuestDialogue)
            {
                sentences.Enqueue(sentence);
            }
        }
        else
        {
            if (_questAsset.State == QuestAsset.QuestState.Inactive)
            {
                foreach (string sentence in _questAsset.InactiveQuestDialogue)
                {
                    sentences.Enqueue(sentence);
                }
            }
            else if (_questAsset.State == QuestAsset.QuestState.Active)
            {
                foreach (string sentence in _questAsset.ActiveQuestDialogue)
                {
                    sentences.Enqueue(sentence);
                }
            }
            else if (_questAsset.State == QuestAsset.QuestState.Completed)
            {
                foreach (string sentence in _questAsset.CompletedQuestDialogue)
                {
                    sentences.Enqueue(sentence);
                }
            }
        }
        DisplayNextSentence();
    }



    IEnumerator FadeObject(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < fadeSpeed)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / fadeSpeed);

            yield return null;
        }
    }
}