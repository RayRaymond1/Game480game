using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyReferencesBoss : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public WordBankBoss wordBank;
    public EnemyController enemyController;
    public Transform target;
    public EventManager eventManagerObject;
    public EventManager eventManagerEnemy;
    public TyperBoss typer;
    public EnemyAIBoss enemyAI;
    [Header("Stats")]
    public float pathUpdateDelay = 0.1f;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        wordBank = GetComponent<WordBankBoss>();
        typer = GetComponent<TyperBoss>();
        enemyAI = GetComponent<EnemyAIBoss>();
        eventManagerEnemy = GetComponent<EventManager>();
        eventManagerEnemy.wordFailedEvent.AddListener(eventManagerObject.wordFailedEvent.Invoke);
        eventManagerEnemy.wordCompleteEvent.AddListener(eventManagerObject.wordCompleteEvent.Invoke);
        eventManagerEnemy.correctLetterEvent.AddListener(eventManagerObject.correctLetterEvent.Invoke);
        eventManagerEnemy.wrongLetterEvent.AddListener(eventManagerObject.wrongLetterEvent.Invoke);
        eventManagerEnemy.wordBankComplete.AddListener(eventManagerObject.wordBankComplete.Invoke);
        //eventManagerEnemy.levelComplete.AddListener(eventManagerObject.levelComplete.Invoke);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
