using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    PlayableDirector director;
    CinemachineBrain brain;
    GameObject playerView;
    public GameObject EnemiesToLoad = null;
    public EventManager eventManager;
    public GameObject canvas;
    public string nextScene;
    
    
    void Start(){
        director = this.GetComponent<PlayableDirector>();
        brain = GameObject.FindFirstObjectByType<CinemachineBrain>();
        playerView = GameObject.Find("PlayerView");
       
        
    }

    public void onCutsceneStart(){
        if(eventManager != null)
            eventManager.cutScene = true;
        if(playerView != null)
            playerView.gameObject.SetActive(false);
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        director.SetGenericBinding(timeline.GetOutputTrack(0), brain);
        if(canvas != null)
            canvas.SetActive(false);
       
     

        }

    public void OnCutsceneEnd(){
        if(playerView != null)
            playerView.gameObject.SetActive(true);
        EnableEnemies();
        if(canvas != null)
            canvas.SetActive(true);
        if(eventManager != null)
            eventManager.cutScene = false;
        
        Destroy(this.gameObject);
    }
    public void EnableEnemies()
    {
        if(EnemiesToLoad == null){
            return;
        }
        EnemiesToLoad.SetActive(true);
    }

    public void onTransitionStart(){
        if(eventManager != null)
            eventManager.cutScene = true;
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        if(playerView != null)
            director.SetGenericBinding(timeline.GetOutputTrack(0), playerView);
        


        if (canvas != null)
            canvas.SetActive(false);
    }

    public void onTransitionEnd(){
        EnableEnemies();
        if(canvas != null)
            canvas.SetActive(true);
        if(eventManager != null)
            eventManager.cutScene = false;
        Destroy(this.gameObject);
    }
    public void loadNextScene(){
        if(nextScene != null)
            SceneManager.LoadScene(nextScene);
    }
}
