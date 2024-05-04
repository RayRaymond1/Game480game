using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;
    public float damage = 20;
    public EventManager eventManagerObject;

    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Awake(){
        eventManagerObject.wordFailedEvent.AddListener(TakeDamage);
        eventManagerObject.wordFailedEvent.AddListener(DamageTaken);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void TakeDamage()
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

    }
    void UpdateHealthBar()
    {
        // Calculate the ratio of currentHealth to maxHealth
        float healthRatio = currentHealth / maxHealth;

        // Update the fillAmount of the health bar
        healthBar.fillAmount = Mathf.Clamp(healthRatio, 0, 1);
    }
    void Die()
    {
        SceneManager.LoadScene("GameOver");
    }
    void DamageTaken()
    {
        Debug.Log("Player took damage");
        Debug.Log("Current Health: " + currentHealth);
    }

}
