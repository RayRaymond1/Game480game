using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* `using UnityEn` is not a valid C# namespace or directive. It seems like you were trying to reference
`UnityEvent` from the `UnityEngine.Events` namespace. To use `UnityEvent` in your script, you need
to include the correct namespace at the beginning of your script like this: */
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    private List<object> EnemyList = new List<object>();
    private int currentEnemy = 0;
    public UnityEvent changeEnemyEvent;
    private bool hasRun = false;

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
            if(!hasRun)
            {
                changeEnemyEvent.Invoke();
                hasRun = true;
            }
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
