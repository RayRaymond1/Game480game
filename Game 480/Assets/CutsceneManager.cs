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

    void Start(){
        director = this.GetComponent<PlayableDirector>();
        brain = GameObject.FindFirstObjectByType<CinemachineBrain>();
        playerView = GameObject.Find("PlayerView");
    }

    public void onCutsceneStart(){
        playerView.gameObject.SetActive(false);
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        director.SetGenericBinding(timeline.GetOutputTrack(0), brain);
    }

    public void OnCutsceneEnd(){
        playerView.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }
}
