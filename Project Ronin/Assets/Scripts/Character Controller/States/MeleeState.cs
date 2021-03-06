using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : StateMachineBehaviour
{
    static int numActive = 0;
    public event System.Action OnLeaveMelee;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       CharacterCaptureController cc = animator.GetComponent<CharacterCaptureController>();
       Vector2 attackVector = new Vector2(animator.GetFloat("xAttack"), animator.GetFloat("yAttack"));
       cc.AttackRotate(attackVector, this, (numActive == 0));

       numActive++;

       animator.GetComponent<ComboManager>().AddCombo();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //    animator.ResetTrigger("attack");
    // }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        numActive--;
        if(numActive != 0) {return;}
        OnLeaveMelee?.Invoke();

       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
