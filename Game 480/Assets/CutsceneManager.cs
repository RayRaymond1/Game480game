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
    
    public AudioSource audioSource;

    void Start(){
        director = this.GetComponent<PlayableDirector>();
        brain = GameObject.FindFirstObjectByType<CinemachineBrain>();
        playerView = GameObject.Find("PlayerView");
        audioSource =  this.GetComponent<AudioSource>(); // Initialize the audio source
        
    }

    public void onCutsceneStart(){
        eventManager.cutScene = true;
        playerView.gameObject.SetActive(false);
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        director.SetGenericBinding(timeline.GetOutputTrack(0), brain);
        if(canvas != null)
            canvas.SetActive(false);
        foreach (var track in timeline.GetOutputTracks()) // Loop through all output tracks
        {
            if (track is AudioTrack) // If the track is an audio track
            {
                director.SetGenericBinding(track, audioSource); // Bind the audio source to the audio track
                
            }
        }

     

        }

    public void OnCutsceneEnd(){
        playerView.gameObject.SetActive(true);
        EnableEnemies();
        if(canvas != null)
            canvas.SetActive(true);
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
        eventManager.cutScene = true;
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        
        director.SetGenericBinding(timeline.GetOutputTrack(0), playerView);
        foreach (var track in timeline.GetOutputTracks()) // Loop through all output tracks
        {
            if (track is AudioTrack) // If the track is an audio track
            {
                director.SetGenericBinding(track, audioSource); // Bind the audio source to the audio track

            }
        }


        if (canvas != null)
            canvas.SetActive(false);
    }

    public void onTransitionEnd(){
        EnableEnemies();
        if(canvas != null)
            canvas.SetActive(true);
        eventManager.cutScene = false;
        Destroy(this.gameObject);
    }
    public void loadNextScene(){
        if(nextScene != null)
            SceneManager.LoadScene(nextScene);
    }
}
