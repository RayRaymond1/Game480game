using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public Image healthBar;
    Typer class1  = new Typer();

    

    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    { // not game prod, should only be for testing
        healthBar.fillAmount = Mathf.Clamp(currentHealth/maxHealth, 0, 1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
    }

}
