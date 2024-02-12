using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Typer : MonoBehaviour
{
    public TMP_Text wordOutput = null;
    public UnityEvent correctLetterEvent;
    public UnityEvent wrongLetterEvent;
    public UnityEvent wordCompleteEvent;
    public UnityEvent wordFailedEvent;

    private string remainingWord = string.Empty;
    private string currentWord = "hello";

    void Start()
    {
        SetCurrentWord();
    }

    void SetCurrentWord()
    {
        SetRemainingWord(currentWord);
    }
    
    private void SetRemainingWord(string newString)
    {
        remainingWord = newString;
        wordOutput.text = remainingWord;
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.anyKeyDown)
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

}
