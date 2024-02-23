using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Typer : MonoBehaviour
{
    public TMP_Text currentWordText = null;
    public TMP_Text currentProgressText = null;
    public WordBank wordBank = null;

    public UnityEvent correctLetterEvent;
    public UnityEvent wrongLetterEvent;
    public UnityEvent wordCompleteEvent;
    public UnityEvent wordFailedEvent;
    public UnityEvent levelComplete;

    private string currentWord = string.Empty;
    private string currentWordProgress = string.Empty;
    [SerializeField] private float originalTime = 100f;
    private float timer = 0f;
    private bool cutScene = false;
    private string nextLetter = string.Empty;

    void Start()
    {
        SetCurrentWord();
    }

    void SetCurrentWord()
    {
        currentWord = wordBank.GetWord();
        nextLetter = currentWord;
        if(string.IsNullOrEmpty(currentWord))
        {
            levelComplete.Invoke();
            return;
        }
        timer = originalTime;
        currentWordText.text = currentWord;
        currentWordProgress = string.Empty;
        currentProgressText.text = currentWordProgress;
    }

    void disableInput()
    {
        cutScene = true;
    }

    void enableInput()
    {
        cutScene = false;
    }
    
    private void SetRemainingWord()
    {
        currentProgressText.text = currentWordProgress;
        nextLetter = nextLetter.Remove(0, 1);
    }

    void Update()
    {
        CheckInput();
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            wordFailedEvent.Invoke();
            SetCurrentWord();
        }
    }

    void CheckInput()
    {
        if(Input.anyKeyDown && !cutScene)
        {
            string keysPressed = Input.inputString;
            
            if(keysPressed.Length == 1)
                EnterLetter(keysPressed);
        }
    }

    void EnterLetter(string typedLetter)
    {
        Debug.Log("Typed letter: " + typedLetter);
        if(IsCorrectLetter(typedLetter))
        {
            AddLetter(typedLetter);
            correctLetterEvent.Invoke();
            if(IsWordComplete())
            {
                wordCompleteEvent.Invoke();
                SetCurrentWord();
            }
        } else{
            //Debug.Log("Wrong letter");
            Debug.Log("Current Progress:" + currentWordProgress);
            wrongLetterEvent.Invoke();
        }
    }

    bool IsCorrectLetter(string letter)
    {
        Debug.Log("Next Letter: " + currentWord[currentWordProgress.Length]);
        Debug.Log(letter[0]);
        Debug.Log(nextLetter[0]);
        return letter[0] == nextLetter[0];
    }
    void AddLetter(string typedLetter)
    {
       // Debug.Log("Adding letter: " + typedLetter);
       // Debug.Log("Current word progress: " + currentWordProgress);
        currentWordProgress += typedLetter;
        SetRemainingWord();
    }

    bool IsWordComplete()
    {
        return currentWordProgress == currentWord;
    }

    public void IncreaseTime(float timeIncrement)
    {
        timer += timeIncrement;
    }
    public float GetTime()
    {
        return timer;
    }

}
