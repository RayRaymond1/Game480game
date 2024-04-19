using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    // List to store all the enemies
    private List<object> EnemyList = new List<object>();
    
    // Index of the current enemy
    private int currentEnemy = 0;
    
    // Reference to the event manager object
    public EventManager eventManagerObject;
    
    // Reference to the boss enemy
    public GameObject Boss = null;
    
    // Vertical displacement for the controller
    public float vertDisplacement = 2.5f;

    // Update is called once per frame
    void Update()
    {
        // If there is a boss
        if(Boss != null)
        {
            // Check if the boss can be attacked
            bool canAttackBoss = CanAttackBoss();
            
            // If the boss can't be attacked and is the current enemy, move to the next enemy
            if(!canAttackBoss && GetCurrentEnemy() == Boss)
            {
                NextEnemy();
            }
        }
        
        // If the space key is pressed, move to the next enemy
        if(Input.GetKeyDown("space"))
        {
            NextEnemy();
        }
        
        // If there are enemies
        if(EnemyList.Count > 0){
            // Get the current enemy
            GameObject currentEnemyObject = GetCurrentEnemy() as GameObject;
            
            // If the current enemy exists
            if(currentEnemyObject != null)
            {
                // Move the controller to the current enemy's position with a vertical displacement
                Vector3 pos = currentEnemyObject.transform.position;
                transform.position = new Vector3(pos.x, pos.y + vertDisplacement, pos.z);
            }
        }else if(!eventManagerObject.cutScene){
            // If there are no enemies and there is no cutscene, the level is complete
            eventManagerObject.levelComplete.Invoke();
        }
    }

    // Add an enemy to the list
    public void AddEnemy(object enemy)
    {
        EnemyList.Add(enemy);
    }
    
    // Remove an enemy from the list
    public void RemoveEnemy(object enemy)
    {
        EnemyList.Remove(enemy);
        
        // If the current enemy index is out of range, reset it to 0
        if(currentEnemy >= EnemyList.Count)
        {
            currentEnemy = 0;
        }
    }
    
    // Get the current enemy
    public object GetCurrentEnemy()
    {
        // If there are no enemies, return null
        if(EnemyList.Count == 0)
        {
            return null;
        }
        
        // Return the current enemy
        return EnemyList[currentEnemy];
    }
    
    // Check if the boss can be attacked
    private bool CanAttackBoss()
    {
        // For each enemy in the list
        foreach(GameObject enemy in EnemyList)
        {
            // If the enemy exists and is not the boss
            if(enemy != null && enemy != Boss)
            {
                // Get the typer component
                Typer typer = enemy.GetComponent<Typer>();
                
                // If the typer exists and the word is not complete, the boss can't be attacked
                if(typer != null && !typer.isWordComplete)
                    return false;
            }
        }
        
        // If all words are complete, the boss can be attacked
        return true;
    }
    
    // Move to the next enemy
    private void NextEnemy()
    {
        // Increment the current enemy index
        currentEnemy++;
        
        // If the current enemy index is out of range, reset it to 0
        if(currentEnemy >= EnemyList.Count)
        {
            currentEnemy = 0;
        }
    }
}