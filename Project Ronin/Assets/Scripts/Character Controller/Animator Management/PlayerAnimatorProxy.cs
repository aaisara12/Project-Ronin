using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Use animator parameters directly!")]
public class PlayerAnimatorProxy : AnimatorProxy
{
    // set the parameters here
    private HashSet<string> combatTriggers = new HashSet<string>(new string[] {
        "attack",
        "parry",
        "charge",
        "shock"
    });

    private HashSet<string> movementFloats = new HashSet<string>(new string[] { 
       "xInput",
       "yInput"
    });

    public override void SetTrigger(string triggerName)
    {
        if (combatTriggers.Contains(triggerName))
        {
            animator.SetBool("combat", true);

            // only keep the latest input
            foreach (var trigger in combatTriggers)
            {
                animator.ResetTrigger(trigger);
            }
        }

        base.SetTrigger(triggerName);
    }

    public override void SetFloat(string floatName, float value)
    { 
        base.SetFloat(floatName, value);

        // look for non-trivial movement
        bool isMoving = false;

        foreach (string name in movementFloats)
        {
            if (Mathf.Abs(animator.GetFloat(name)) > 0)
            {
                isMoving = true;
                break;
            }
        }

        animator.SetBool("moving", isMoving);
    }
}
