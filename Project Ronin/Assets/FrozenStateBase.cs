using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor.Animations;

/// <summary>
/// Out of box it helps the state machine ignore input for a certain portion of the current state 
/// (usually to stop player interrupt the animation).
/// "Frozen" == ignore inputs, achieved by wiping out triggers right before exit time hits.
/// </summary>
public class FrozenStateBase : StateMachineBehaviour
{
    /// <summary>
    /// Time stamp to unlock the character for player inputs. Normalized (0 - 100).
    /// </summary>
    [SerializeField]
    private float normalizedUnlockTime = 70;

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
            foreach (var param in animator.parameters)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }
}
