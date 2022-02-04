using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAimState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // rotate using euler
        Vector3 direction = Input.mousePosition;
        direction -= new Vector3(Screen.width / 2, Screen.height / 2);
        if (direction.magnitude > 0)
        {
            direction.Normalize();
            float zAngle = Vector3.SignedAngle(new Vector3(0, 1, 0), direction, new Vector3(0, 0, 1));
            Vector3 rotationEuler = new Vector3(0, -zAngle, 0);

            animator.transform.rotation = Quaternion.Euler(rotationEuler);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
