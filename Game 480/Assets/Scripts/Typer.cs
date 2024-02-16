using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Typer : MonoBehaviour
{
    public TMP_Text wordOutput = null;
    public WordBank wordBank = null;

    public UnityEvent correctLetterEvent;
    public UnityEvent wrongLetterEvent;
    public UnityEvent wordCompleteEvent;
    public UnityEvent wordFailedEvent;
    public UnityEvent levelComplete;

    private string remainingWord = string.Empty;
    private string currentWord = string.Empty;
    private float timer = 10f;
    private bool cutScene = false;

    void Start()
    {
        SetCurrentWord();
    }

    void SetCurrentWord()
    {
        currentWord = wordBank.GetWord();
        timer = 10f;
        SetRemainingWord(currentWord);
    }

    void disableInput()
    {
        cutScene = true;
    }

    void enableInput()
    {
        cutScene = false;
    }
    
    private void SetRemainingWord(string newString)
    {
        remainingWord = newString;
        if(string.IsNullOrEmpty(remainingWord))
        {
            levelComplete.Invoke();
        }
        wordOutput.text = remainingWord;
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
        if(IsCorrectLetter(typedLetter))
        {
            RemoveLetter();
            correctLetterEvent.Invoke();
            if(IsWordComplete())
            {
                wordCompleteEvent.Invoke();
                SetCurrentWord();
            }
        } else{
            Debug.Log("Wrong letter");
            wrongLetterEvent.Invoke();
        }
    }

    bool IsCorrectLetter(string letter)
    {
        return remainingWord.IndexOf(letter) == 0;
    }
    void RemoveLetter()
    {
        string newString = remainingWord.Remove(0, 1);
        SetRemainingWord(newString);
    }

    bool IsWordComplete()
    {
        return remainingWord.Length == 0;
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
