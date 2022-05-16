using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class used to prevent animator attack parameter spamming
public class AttackInputProxy : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool isAttackInputEnabled = true;

    [SerializeField] int numLocking = 0;

    // Called on particular frames of animation
    public void RequestUnlockAttackInput()
    {
        numLocking--;
        if(numLocking == 0)
            isAttackInputEnabled = true;
        //Debug.Log("Enabled attack");
    }

    public void LockAttackInput()
    {
        numLocking++;
        isAttackInputEnabled = false;
        //Debug.Log("Disabled attack");
    }

    public void RequestAttack()
    {
        if(isAttackInputEnabled)
        {
            animator.SetTrigger("attack");
        }
    }
}
