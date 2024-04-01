using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyReferences : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public WordBank wordBank;
    public EnemyController enemyController;
    public Transform target;
    public EventManager eventManagerObject;
    public EventManager eventManagerEnemy;
    public Typer typer;
    public EnemyAI enemyAI;
    [Header("Stats")]
    public float pathUpdateDelay = 0.1f;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        wordBank = GetComponent<WordBank>();
        typer = GetComponent<Typer>();
        enemyAI = GetComponent<EnemyAI>();
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
