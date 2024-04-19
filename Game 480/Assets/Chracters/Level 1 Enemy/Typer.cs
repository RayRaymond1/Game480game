using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Typer : MonoBehaviour
{
    // References to other components and objects used by this script
    private EnemyReferences enemyReferences;

    // UI elements to display the current word and progress
    public TMP_Text currentWordText = null;
    public TMP_Text currentProgressText = null;

    // The current word and progress
    public string currentWord = string.Empty;
    private string currentWordProgress = string.Empty;

    // The next letter to be typed
    private string nextLetter = string.Empty;

    // The player's score
    public int score = 0;

    // Flag to indicate if the current word is complete
    public bool isWordComplete = false;

    // Initialize references
    void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    // Initialize the current word and add listeners for events
    void Start()
    {
        SetCurrentWord();
        EnemyLoaded();
        enemyReferences.eventManagerEnemy.wordCompleteEvent.AddListener(CalculateScore);
        enemyReferences.eventManagerEnemy.wordFailedEvent.AddListener(ResetWordFail);
        enemyReferences.eventManagerEnemy.wordBankComplete.AddListener(OnDestroy);
        enemyReferences.eventManagerEnemy.ResetWord.AddListener(SetCurrentWord);
    }

    // Set the current word
    void SetCurrentWord()
    {
        isWordComplete = false;
        currentWord = enemyReferences.wordBank.GetWord();
        nextLetter = currentWord;
        if(string.IsNullOrEmpty(currentWord))
        {
            enemyReferences.eventManagerEnemy.wordBankComplete.Invoke();
            return;
        }
        currentWordText.text = currentWord;
        currentWordProgress = string.Empty;
        currentProgressText.text = currentWordProgress;
    }

    // Add this enemy to the controller's list
    void EnemyLoaded()
    {
        enemyReferences.enemyController.AddEnemy(transform.gameObject);
    }

    // Update the remaining word
    private void SetRemainingWord()
    {
        currentProgressText.text = currentWordProgress;
        nextLetter = nextLetter.Remove(0, 1);
    }

    // Check for input each frame
    void Update()
    {
        CheckInput();
    }

    // Check if the player has typed a letter
    void CheckInput()
    {
        if (!isWordComplete && canInput() && Input.inputString != string.Empty)
        {
            string keysPressed = Input.inputString;

            if (keysPressed.Length == 1)
                EnterLetter(keysPressed);
        }
    }

    // Check if the player can input a letter
    bool canInput()
    {
        if (isWordComplete)
            return false;
        if(enemyReferences.enemyAI == null)
            return Input.anyKeyDown && enemyReferences.enemyController.GetCurrentEnemy() == transform.gameObject;
        return Input.anyKeyDown && !enemyReferences.enemyAI.reseting && enemyReferences.enemyController.GetCurrentEnemy() == transform.gameObject && !enemyReferences.enemyAI.inRange;
    }

    // Reset the word when the player fails
    void ResetWordFail()
    {
        currentWordText.text = currentWord;
        currentWordProgress = string.Empty;
        currentProgressText.text = currentWordProgress;
        nextLetter = currentWord;
    }

    // Process the letter the player has typed
    void EnterLetter(string typedLetter)
    {
        if(IsCorrectLetter(typedLetter))
        {
            AddLetter(typedLetter);
            enemyReferences.eventManagerEnemy.correctLetterEvent.Invoke();
            if(IsWordComplete())
            {
                enemyReferences.eventManagerEnemy.wordCompleteEvent.Invoke();
            }
        } else{
            enemyReferences.eventManagerEnemy.wrongLetterEvent.Invoke();
        }
    }

    // Check if the letter the player has typed is correct
    bool IsCorrectLetter(string letter)
    {
        return letter[0] == nextLetter[0];
    }

    // Add the letter the player has typed to the current progress
    void AddLetter(string typedLetter)
    {
        currentWordProgress += typedLetter;
        SetRemainingWord();
    }

    // Check if the current word is complete
    bool IsWordComplete()
    {
        isWordComplete = currentWordProgress == currentWord;

        return isWordComplete;
    }

    // Calculate the score when a word is completed
    public void CalculateScore()
    {
        enemyReferences.eventManagerObject.score += Mathf.FloorToInt(currentWord.Length * Vector3.Distance(transform.position, enemyReferences.target.position));
    }

    // Remove this enemy from the controller's list and destroy it when the word bank is complete
    private void OnDestroy()
    {
        enemyReferences.enemyController.RemoveEnemy(transform.gameObject);
        Destroy(transform.gameObject);
    }
}