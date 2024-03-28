using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject _dialogueUI;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text dialogueName;
    [SerializeField] TMP_Text dialogueButtonText;
    [SerializeField] float fadeSpeed;
    [SerializeField] GameManager _gameManager;
    private Queue<string> sentences;


    // Basic Typewriter Functionality
    int _currentVisibleCharacterIndex;
    Coroutine _typewriterCoroutine;
    bool _readyForNewText = true;

    WaitForSeconds _simpleDelay;
    WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] float charactersPerSecond = 20;
    [SerializeField] float interpunctuationDelay = 0.5f;

    //Skipping Functionality
    public bool CurrentlySkipping { get; private set; }
    WaitForSeconds _skipDelay;

    [Header("Skip Options")]
    [SerializeField] bool quickSkip;
    [SerializeField]
    [Min(1)] int skipSpeedup = 5;

    //Event Functionality
    WaitForSeconds _textboxFullEventDelay;
    [SerializeField]
    [Range(0.1f, 0.5f)] float sendDoneDelay = 0.25f;

    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;

    public void Start()
    {
        sentences = new Queue<string>();
        _dialogueUI.SetActive(false);
    }

    public void Awake()
    {
        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / charactersPerSecond * skipSpeedup);
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }


    public void StartDialogue(Dialogue dialogue)
    {
        _gameManager.LoadState("Dialogue");
        sentences.Clear();
        _dialogueUI.SetActive(true);
        //StartCoroutine(FadeObject(canvGroup, canvGroup.alpha, isFaded ? 1 : 0));
        //isFaded = !isFaded;

        dialogueName.text = dialogue.name;
        dialogueButtonText.text = dialogue.continueDialogue;
        //endDialogue = dialogue.endDialogue;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        PrepareForNewText();
    }

    private void PrepareForNewText()
    {
        if (!_readyForNewText || dialogueText.maxVisibleCharacters >= dialogueText.textInfo.characterCount)
            return;

        CurrentlySkipping = false;
        _readyForNewText = false;

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        dialogueText.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        string sentence = sentences.Dequeue();

        _typewriterCoroutine = StartCoroutine(Typewriter(sentence));
    }

    private IEnumerator Typewriter(string sentence)
    {
        dialogueText.text = sentence;

        TMP_TextInfo textInfo = dialogueText.textInfo;

        while (_currentVisibleCharacterIndex < textInfo.characterCount)
        {
            var lastCharacterIndex = textInfo.characterCount - 1;

            if(_currentVisibleCharacterIndex >= lastCharacterIndex)
            {
                dialogueText.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNewText = true;
                yield break;
            }

            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;
            dialogueText.maxVisibleCharacters++;

            if (!CurrentlySkipping &&
                    (character == '?' || character == '.' || character == ',' || character == ':' ||
                     character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
            }

            CharacterRevealed?.Invoke(character);
            _currentVisibleCharacterIndex++;
        }
    }

    #region SkipFunctionality
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (dialogueText.maxVisibleCharacters != dialogueText.textInfo.characterCount - 1)
                Skip();
        }
    }

    private void Skip(bool quickSkipNeeded = false)
    {
        if (CurrentlySkipping)
            return;

        CurrentlySkipping = true;

        if (!quickSkip || !quickSkipNeeded)
        {
            StartCoroutine(SkipSpeedupReset());
            return;
        }

        StopCoroutine(_typewriterCoroutine);
        dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
        _readyForNewText = true;
        CompleteTextRevealed?.Invoke();
    }

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => dialogueText.maxVisibleCharacters == dialogueText.textInfo.characterCount - 1);
        CurrentlySkipping = false;
    }
    #endregion

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