using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    private List<object> EnemyList = new List<object>();
    private int currentEnemy = 0;
    public EventManager eventManagerObject;
    public GameObject Boss = null;
    public float vertDisplacement = 2.5f;
    void Update()
    {
        if(Boss != null)
        {
            bool canAttackBoss = CanAttackBoss();
            if(!canAttackBoss && GetCurrentEnemy() == Boss)
            {
                NextEnemy();
            }
        }
        if(Input.GetKeyDown("space"))
        {
            NextEnemy();
        }
        if(EnemyList.Count > 0){
            GameObject currentEnemyObject = GetCurrentEnemy() as GameObject;
            if(currentEnemyObject != null)
            {
                Vector3 pos = currentEnemyObject.transform.position;
                transform.position = new Vector3(pos.x, pos.y + vertDisplacement, pos.z);
            }
        }else if(!eventManagerObject.cutScene){
            eventManagerObject.levelComplete.Invoke();
        }
    }

    public void AddEnemy(object enemy)
    {
        EnemyList.Add(enemy);
    }
    public void RemoveEnemy(object enemy)
    {
        EnemyList.Remove(enemy);
        if(currentEnemy >= EnemyList.Count)
        {
            currentEnemy = 0;
        }
    }
    public object GetCurrentEnemy()
    {
        if(EnemyList.Count == 0)
        {
            return null;
        }
        return EnemyList[currentEnemy];
    }
    private bool CanAttackBoss()
    {
        foreach(GameObject enemy in EnemyList)
        {
            if(enemy != null && enemy != Boss)
            {
                Typer typer = enemy.GetComponent<Typer>();
                if(typer != null && !typer.isWordComplete)
                    return false;
            }
        }
        return true;
    }
    private void NextEnemy()
    {
        currentEnemy++;
        if(currentEnemy >= EnemyList.Count)
        {
            currentEnemy = 0;
        }
    }
}
