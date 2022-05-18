using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class used to prevent animator attack parameter spamming
public class AnimatorTriggerProxy : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool isInputUnlocked = true;

    [SerializeField] int numLocking = 0;

    // Called on particular frames of animation
    public void ReleaseInputLock()
    {
        numLocking--;
        if(numLocking == 0)
            isInputUnlocked = true;
        //Debug.Log("Enabled attack");
    }

    public void AddInputLock()
    {
        numLocking++;
        isInputUnlocked = false;
        //Debug.Log("Disabled attack");
    }

    public void RequestTrigger(string triggerName)
    {
        if(isInputUnlocked)
        {
            animator.SetTrigger(triggerName);
        }
    }

    // Useful for restabilizing state when animations are interrupted and certain lock requests are missed
    public void ResetLocks()
    {
        numLocking = 0;
        isInputUnlocked = true;
    }
}
