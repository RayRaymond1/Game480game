using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<object> EnemyList = new List<object>();
    private int currentEnemy = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            currentEnemy++;
            if(currentEnemy >= EnemyList.Count)
            {
                currentEnemy = 0;
            }
        }
        GameObject currentEnemyObject = GetCurrentEnemy() as GameObject;
        if(currentEnemyObject != null)
        {
            Vector3 pos = currentEnemyObject.transform.position;
            transform.position = new Vector3(pos.x, pos.y + 2, pos.z);
        }
        if(currentEnemyObject == null)
        {
            transform.position = new Vector3(0, 1000, 0);
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
}
