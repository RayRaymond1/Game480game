using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseiEnemyDriver : MonoBehaviour
{
    public Animator senseiAnimator;

    public void Attack(){
        senseiAnimator.SetBool("Attacking", true);

        //senseiAnimator.SetBool("Attacking", false);

    }

    public void AttackReset(){
        senseiAnimator.SetBool("Attacking", false);
    }
}
