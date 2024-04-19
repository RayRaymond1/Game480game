using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
        healthBar.fillAmount = Mathf.Clamp(currentHealth/maxHealth, 0, 1);
    }
    void TakeDamage()
    {
        currentHealth -= damage;
    }
    void DamageTaken(){
        Debug.Log("Damage Taken");
        Debug.Log(currentHealth);
    }

}
