using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        base.SetTrigger(triggerName);

        if (combatTriggers.Contains(triggerName))
        {
            animator.SetBool("combat", true);
        }
    }

    public override void SetFloat(string floatName, float value)
    { 
        base.SetFloat(floatName, value);

        // look for non-trivial movement
        bool isMoving = false;

        foreach (string name in movementFloats)
        {
            if (animator.GetFloat(name) > 0)
            {
                isMoving = true;
                break;
            }
        }

        animator.SetBool("moving", isMoving);
    }
}
