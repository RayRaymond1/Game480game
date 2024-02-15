using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    public Typer typer;
    public Camera mainCamera;
    public UnityEvent sceneStart;
    public float shoveDistance = 1f;     public float shoveTimeIncrease = 5f; 
    private Vector3 initialPosition; 
    private Vector3 stopPoint;
    public float stopDistance = 3f; 
    private bool reseting = false;
    public float jumpBackTime = 0.5f;

    void Awake()
    {
        sceneStart.AddListener(OnSceneStart);
        typer.wordFailedEvent.AddListener(wordFailed);
    }

    void OnSceneStart()
    {
        initialPosition = transform.position;
        stopPoint = mainCamera.transform.position - transform.position;
        stopPoint = stopPoint - stopPoint.normalized * stopDistance;
    }
    
    void wordFailed(){
        StartCoroutine(JumpBack(initialPosition));
    }

    void Start(){
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

    IEnumerator JumpBack(Vector3 endPosition)
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