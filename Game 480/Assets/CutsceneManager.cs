using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoBehaviour
{
    PlayableDirector director;
    CinemachineBrain brain;
    GameObject playerView;
    public GameObject EnemiesToLoad = null;
    public EventManager eventManager;
    public GameObject canvas;

    void Start(){
        director = this.GetComponent<PlayableDirector>();
        brain = GameObject.FindFirstObjectByType<CinemachineBrain>();
        playerView = GameObject.Find("PlayerView");
    }

    public void onCutsceneStart(){
        eventManager.cutScene = true;
        playerView.gameObject.SetActive(false);
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        director.SetGenericBinding(timeline.GetOutputTrack(0), brain);
        if(canvas != null)
            canvas.SetActive(false);
    }

    public void OnCutsceneEnd(){
        eventManager.cutScene = false;
        playerView.gameObject.SetActive(true);
        Destroy(this.gameObject);
        EnableEnemies();
        if(canvas != null)
            canvas.SetActive(true);
    }
    public void EnableEnemies()
    {
        if(EnemiesToLoad == null){
            return;
        }
        EnemiesToLoad.SetActive(true);
    }

    public void onTransitionStart(){
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        
        director.SetGenericBinding(timeline.GetOutputTrack(0), playerView);
    }

    public void onTransitionEnd(){
        Destroy(this.gameObject);
    }
}
