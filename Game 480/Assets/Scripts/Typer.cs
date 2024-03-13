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
    public UnityEvent wordBankComplete;

    private string currentWord = string.Empty;
    private string currentWordProgress = string.Empty;
    [SerializeField] private float originalTime = 1f;
    private float timer = 0f;
    private bool cutScene = false;
    private string nextLetter = string.Empty;
    public EnemyController enemyController;
    private int score = 0;
    private int health = 100;
    [SerializeField] private bool multipleWords = false;

    void Start()
    {
        SetCurrentWord();
        EnemyLoaded();
        wordCompleteEvent.AddListener(CalculateScore);
        wordFailedEvent.AddListener(decreaseHealth);
        wordBankComplete.AddListener(OnDestroy);
    }

    void SetCurrentWord()
    {
        currentWord = wordBank.GetWord();
        nextLetter = currentWord;
        if(string.IsNullOrEmpty(currentWord))
        {
            wordBankComplete.Invoke();
            return;
        }
        timer = originalTime * currentWord.Length;
        currentWordText.text = currentWord;
        currentWordProgress = string.Empty;
        currentProgressText.text = currentWordProgress;
    }
    void EnemyLoaded()
    {
        enemyController.AddEnemy(transform.parent.gameObject);
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
            ResetWord();
        }
    }

    void CheckInput()
    {
        if(Input.anyKeyDown && !cutScene && enemyController.GetCurrentEnemy() == transform.parent.gameObject)
        {
            string keysPressed = Input.inputString;
            
            if(keysPressed.Length == 1)
                EnterLetter(keysPressed);
        }
    }
    void ResetWord()
    {
        timer = originalTime * currentWord.Length;
        currentWordText.text = currentWord;
        currentWordProgress = string.Empty;
        currentProgressText.text = currentWordProgress;
    }

    void EnterLetter(string typedLetter)
    {
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
            wrongLetterEvent.Invoke();
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
    private void CalculateScore()
    {
        score += Mathf.FloorToInt(currentWord.Length * timer);
    }
    private void OnDestroy()
    {
        enemyController.RemoveEnemy(transform.parent.gameObject);
        Destroy(transform.parent.gameObject);
    }
    private void decreaseHealth()
    {
        health -= 0;
        Debug.Log("Health: " + health);
        if(health <= 0)
        {
            Debug.Log("Game Over");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
