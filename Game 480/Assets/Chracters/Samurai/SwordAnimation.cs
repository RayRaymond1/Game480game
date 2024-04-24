using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public EventManager eventManager;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip attackSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        eventManager.wordCompleteEvent.AddListener(Attack);
    }

    void Attack(){
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(attackSound);
    }
}
