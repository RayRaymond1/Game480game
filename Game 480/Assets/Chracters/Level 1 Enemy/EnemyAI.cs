using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    // References to other components and objects used by this script
    private EnemyReferences enemyReferences;

    // The time at which the path to the target will be updated
    private float pathUpdateDeadline;

    // The distance at which the enemy will stop moving towards the target
    private float stopDistance;

    // The position where the enemy started
    private Vector3 startPosition;

    // Flags to control the enemy's behavior
    public bool reseting = false;
    private bool newWord = false;
    public bool inRange = false;
    public float speedScalar = 7f;
    private bool attacking = false;
    
    // Initialize references and starting position
    void Awake(){
        enemyReferences = GetComponent<EnemyReferences>(); 
        startPosition = transform.position;
    }

    // Initialize stop distance and add listener for word completion event
    void Start(){
        stopDistance = enemyReferences.agent.stoppingDistance;
        enemyReferences.eventManagerEnemy.wordCompleteEvent.AddListener(CompleteWord);
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a target
        if(enemyReferences.target != null){
            // Check if the target is in range
            inRange = Vector3.Distance(transform.position, enemyReferences.target.position) <= stopDistance + .6f;

            // If the enemy is resetting, move towards the start position
            if(reseting){
                attacking = false;
                UpdatePath(startPosition);
            } else{
                // Otherwise, move towards the target
                UpdatePath(enemyReferences.target.position);

                // If a new word has been completed, reset the word
                if(newWord){
                    newWord = false;
                    enemyReferences.eventManagerEnemy.ResetWord.Invoke();
                }
            }

            // If the enemy has reached the start position, stop resetting
            if(Vector3.Distance(transform.position, startPosition) <= 0.4f){
                reseting = false;
            }

            // Make the enemy look at the target
            LookAtTarget();

            // If the target is in range and the enemy is not attacking or resetting, start attacking
            if(inRange && !attacking && !reseting){
                AttackStart();
            }

            // Update the animator's parameters
            enemyReferences.animator.SetBool("Attacking", inRange);
            enemyReferences.animator.SetBool("Reseting", reseting);

            // Set the speed of the enemy based on the length of the current word
            if(enemyReferences.typer.currentWord.Length > 0 && !reseting){
                setSpeed(1f/enemyReferences.typer.currentWord.Length * speedScalar);
            }else if (reseting){
                setSpeed(7);
            }else {
                setSpeed(1);
            }
        }
    }

    // Make the enemy look at the target
    void LookAtTarget(){
        Vector3 lookPos = (enemyReferences.target.position - transform.position);
        lookPos.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
    }

    // Update the path to the target
    void UpdatePath(Vector3 targetPosition){
        if(reseting){
            enemyReferences.agent.stoppingDistance = 0;
        }else{
            enemyReferences.agent.stoppingDistance = stopDistance;
        }
        if(Time.time >= pathUpdateDeadline){
            pathUpdateDeadline = Time.time + enemyReferences.pathUpdateDelay;
            enemyReferences.agent.SetDestination(targetPosition);
        }
    }

    // End the attack and start resetting
    void AttackEnd(){
        enemyReferences.animator.SetBool("Attacking", false);
        reseting = true;
        attacking = false;
    }

    // Start the attack
    void AttackStart(){
        enemyReferences.audioSource.PlayOneShot(enemyReferences.attackSound);
        enemyReferences.eventManagerEnemy.wordFailedEvent.Invoke();
        attacking = true;
    }

    // Reset the enemy when a word is completed
    void CompleteWord(){
        reseting = true;
        newWord = true;
    }

    // Set the speed of the enemy
    void setSpeed(float speed){
        enemyReferences.agent.speed = speed;
    }
}