using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoSwitch : MonoBehaviour
{
     public EnemyController enemyController;
     public List<GameObject> Enemies = new List<GameObject> {};

    // Start is called before the first frame update
    void Start()
    {
        enemyController.changeEnemyEvent.AddListener(switchStates);
    }
    void switchStates(){
        Debug.Log("Switching States");
        for(int i = 0; i < Enemies.Count; i++){
            if (!Enemies[i].activeInHierarchy) {
                Enemies[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
