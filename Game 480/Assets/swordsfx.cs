using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordsfx : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        animator.SetBool("Word Complete", true);

    }
}
