using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Out of box it helps the state machine ignore input for a certain portion of the current state 
/// (usually to stop player interrupt the animation).
/// "lock" == ignore inputs, achieved by wiping out triggers right before exit time hits.
/// 
/// 
/// No coroutine in state machine behaviour hello???
/// </summary>
public class CombatState : StateMachineBehaviour
{
    /// <summary>
    /// Time stamp to unlock the character for player inputs. Normalized (0 - 100).
    /// </summary>
    [SerializeField]
    private float normalizedUnlockTime = 0.5f;

    private bool locked = true;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        locked = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= normalizedUnlockTime && locked)
        {
            locked = false;
            
            // clean up any one-time input
            foreach (var param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                    animator.ResetTrigger(param.name);
            }

            animator.SetBool("combat", false);
        }
    }
}
