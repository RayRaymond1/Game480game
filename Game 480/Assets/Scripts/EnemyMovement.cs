/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    public Typer typer;
    public GameObject mainCamera;
    public float shoveDistance = 1f;     
    public float shoveTimeIncrease = 5f; 
    private Vector3 initialPosition; 
    private Vector3 stopPoint;
    public float stopDistance = 3f; 
    private bool reseting = false;
    public float jumpBackTime = 0.5f;

    public int jumpBackWaitTime;
    public UnityEvent reachPlayer;
    public UnityEvent attackEnd;

    void Awake()
    {
        //Gets position of the enemy and the player and calculate where the enemy should stop
        initialPosition = transform.position;
        stopPoint = mainCamera.transform.position - initialPosition;
        stopPoint = stopPoint - stopPoint.normalized * stopDistance;
        stopPoint = stopPoint + initialPosition;
        //Add functions to the events
        typer.wordFailedEvent.AddListener(wordFailed);
        typer.wordCompleteEvent.AddListener(wordCompleted);
        attackEnd.AddListener(JumpBack);
    }
    //Functions for when the word is completed or failed
    void wordFailed(){
        StartCoroutine(WordFailedCoroutine());
        
    }
    IEnumerator WordFailedCoroutine(){
        reachPlayer.Invoke();
        yield return new WaitForSeconds(jumpBackWaitTime);
        attackEnd.Invoke();
    }
    void wordCompleted(){
        JumpBack();
    }

    void Update()
    { 
        //Moves the enemy to the stop point
        if(!reseting){
            float distance = Vector3.Distance(transform.position, stopPoint);
            float speed = distance / typer.GetTime();
            transform.position = Vector3.MoveTowards(transform.position, stopPoint, speed * Time.deltaTime);
        }
    }
    //NOT IMPLEMENTED YET
    public void Shove()
    {
        transform.position = transform.position + new Vector3(0, 0, shoveDistance);
        typer.IncreaseTime(shoveTimeIncrease);
    }
    //Jump back to the initial position
    void JumpBack()
    {
        StartCoroutine(JumpBackCoroutine(initialPosition));
    }
    IEnumerator JumpBackCoroutine(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        float progress = 0;
        reseting = true;
        while (progress < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            progress += Time.deltaTime / jumpBackTime;
            yield return null;
        }

        transform.position = endPosition;
        reseting = false;
    }

}
*/