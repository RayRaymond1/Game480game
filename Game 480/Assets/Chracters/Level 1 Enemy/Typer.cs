using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Typer : MonoBehaviour
{
    private EnemyReferences enemyReferences;
    public TMP_Text currentWordText = null;
    public TMP_Text currentProgressText = null;
    public string currentWord = string.Empty;
    private string currentWordProgress = string.Empty;
    private string nextLetter = string.Empty;
    public int score = 0;
    private int health = 100;
    public bool isWordComplete = false;
    void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    void Start()
    {
        SetCurrentWord();
        EnemyLoaded();
        enemyReferences.eventManagerEnemy.wordCompleteEvent.AddListener(CalculateScore);
        enemyReferences.eventManagerEnemy.wordFailedEvent.AddListener(decreaseHealth);
        enemyReferences.eventManagerEnemy.wordFailedEvent.AddListener(ResetWordFail);
        enemyReferences.eventManagerEnemy.wordBankComplete.AddListener(OnDestroy);
        enemyReferences.eventManagerEnemy.ResetWord.AddListener(SetCurrentWord);
    }

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
    void EnemyLoaded()
    {
        enemyReferences.enemyController.AddEnemy(transform.gameObject);
    }
    
    private void SetRemainingWord()
    {
        currentProgressText.text = currentWordProgress;
        nextLetter = nextLetter.Remove(0, 1);
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (!isWordComplete && canInput() && Input.inputString != string.Empty)
        {
            string keysPressed = Input.inputString;

            if (keysPressed.Length == 1)
                EnterLetter(keysPressed);
        }
    }
    bool canInput()
    {
        if (isWordComplete)
            return false;
        if(enemyReferences.enemyAI == null)
            return Input.anyKeyDown && enemyReferences.enemyController.GetCurrentEnemy() == transform.gameObject;
        return Input.anyKeyDown && !enemyReferences.enemyAI.reseting && enemyReferences.enemyController.GetCurrentEnemy() == transform.gameObject && !enemyReferences.enemyAI.inRange;
    }
    void ResetWordFail()
    {
        currentWordText.text = currentWord;
        currentWordProgress = string.Empty;
        currentProgressText.text = currentWordProgress;
        nextLetter = currentWord;
    }

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

    bool IsCorrectLetter(string letter)
    {
        return letter[0] == nextLetter[0];
    }
    void AddLetter(string typedLetter)
    {
        currentWordProgress += typedLetter;
        SetRemainingWord();
    }

    bool IsWordComplete()
    {
        isWordComplete = currentWordProgress == currentWord;

        return isWordComplete;
    }
    public void CalculateScore()
    {
        score += Mathf.FloorToInt(currentWord.Length * Vector3.Distance(transform.position, enemyReferences.target.position));
    }
    private void OnDestroy()
    {
        enemyReferences.enemyController.RemoveEnemy(transform.gameObject);
        Destroy(transform.gameObject);
    }
    private void decreaseHealth()
    {
        health -= 0;
        Debug.Log("Health: " + health);
        if(health <= 0)
        {
            Debug.Log("Game Over");
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
