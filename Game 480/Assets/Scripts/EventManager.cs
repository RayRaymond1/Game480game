using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent wordFailedEvent;
    public UnityEvent wordCompleteEvent;
    public UnityEvent ResetWord;
    public UnityEvent correctLetterEvent;
    public UnityEvent wrongLetterEvent;
    public UnityEvent wordBankComplete;
    public UnityEvent levelComplete;
    public List<GameObject> cutscenesToStart;
    public bool cutScene = false;
    private int nextCutscene = 0;
    void Start(){
        levelComplete.AddListener(SwitchEvents);
    }
    void SwitchEvents(){
        cutscenesToStart[nextCutscene].SetActive(true);
        nextCutscene++;
    }
}
