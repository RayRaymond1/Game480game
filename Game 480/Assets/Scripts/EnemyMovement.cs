using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    public Typer typer;
    public GameObject mainCamera;
    public UnityEvent sceneStart;
    public float shoveDistance = 1f;     
    public float shoveTimeIncrease = 5f; 
    private Vector3 initialPosition; 
    private Vector3 stopPoint;
    public float stopDistance = 3f; 
    private bool reseting = false;
    public float jumpBackTime = 0.5f;
    public EnemyController enemyController;
    public UnityEvent reachPlayer;
    public UnityEvent attackEnd;

    void Awake()
    {
        sceneStart.AddListener(OnSceneStart);
        typer.wordFailedEvent.AddListener(wordFailed);
        typer.wordCompleteEvent.AddListener(wordCompleted);
        attackEnd.AddListener(JumpBack(initialPosition));
    }

    void OnSceneStart()
    {
        initialPosition = transform.position;
        stopPoint = mainCamera.transform.position - initialPosition;
        stopPoint = stopPoint - stopPoint.normalized * stopDistance;
        stopPoint = stopPoint + initialPosition;
    }
    
    void wordFailed(){
        StartCoroutine(WordFailedCoroutine());
    }
    IEnumerator WordFailedCoroutine(){
        reachPlayer.Invoke();
        yield return new WaitForSeconds(1);
        attackEnd.Invoke();
    }
    void wordCompleted(){
        JumpBack(initialPosition);
    }

    void Start()
    {
        sceneStart.Invoke();
    }

    void Update()
    { 
        if(!reseting){
            float distance = Vector3.Distance(transform.position, stopPoint);
            float speed = distance / typer.GetTime();
            transform.position = Vector3.MoveTowards(transform.position, stopPoint, speed * Time.deltaTime);
        }
    }

    public void Shove()
    {
        transform.position = transform.position + new Vector3(0, 0, shoveDistance);
        typer.IncreaseTime(shoveTimeIncrease);
    }

    void JumpBack(Vector3 endPosition)
    {
        StartCoroutine(JumpBackCoroutine(endPosition));
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