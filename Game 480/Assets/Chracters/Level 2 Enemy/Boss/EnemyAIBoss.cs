using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAIBoss : MonoBehaviour
{
    private EnemyReferencesBoss enemyReferences;
    private float pathUpdateDeadline;
    private float stopDistance;
    private Vector3 startPosition;
    public bool reseting = false;
    private bool newWord = false;
    public bool inRange = false;
    public float speedScalar = 6f;
    public List<GameObject> EnemyList = new List<GameObject>();
    private bool attacking = false;
    
    void Awake(){
        enemyReferences = GetComponent<EnemyReferencesBoss>(); 
        startPosition = transform.position;
    }
    void Start(){
        stopDistance = enemyReferences.agent.stoppingDistance;
        enemyReferences.eventManagerEnemy.wordCompleteEvent.AddListener(CompleteWord);
    }
    void Update()
    {
    if(enemyReferences.target != null){
        Debug.Log(Vector3.Distance(transform.position, enemyReferences.target.position));
        Debug.Log(stopDistance + .45f);
        inRange = Vector3.Distance(transform.position, enemyReferences.target.position) <= stopDistance + .6f;
        if(reseting){
            UpdatePath(startPosition);
        } else{
            UpdatePath(enemyReferences.target.position);
            if(newWord){
                newWord = false;
                enemyReferences.eventManagerEnemy.ResetWord.Invoke();
                foreach(GameObject enemy in EnemyList){
                    enemy.GetComponent<EnemyReferences>().eventManagerEnemy.ResetWord.Invoke();
                }
            }
        }
        if(Vector3.Distance(transform.position, startPosition) <= 0.3f){
            reseting = false;
        }
        LookAtTarget();
        if(inRange && !attacking && !reseting){
            AttackStart();
        }
        enemyReferences.animator.SetBool("Attacking", inRange);
        enemyReferences.animator.SetBool("Reseting", reseting)
        ;
        if(enemyReferences.typer.currentWord.Length > 0 && !reseting){
            setSpeed(1f/enemyReferences.typer.currentWord.Length * speedScalar);
        }else if (reseting){
            setSpeed(7);
        }else {
            setSpeed(1);
        }
    }
}
    void LookAtTarget(){
        Vector3 lookPos = (enemyReferences.target.position - transform.position);
        lookPos.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
    }
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
    void AttackEnd(){
        enemyReferences.animator.SetBool("Attacking", false);
        reseting = true;
        attacking = false;
    }
    void AttackStart(){
        enemyReferences.eventManagerEnemy.wordFailedEvent.Invoke();
        attacking = true;
    }
    void CompleteWord(){
        reseting = true;
        newWord = true;
    }
    void setSpeed(float speed){
        enemyReferences.agent.speed = speed;
    }
}
