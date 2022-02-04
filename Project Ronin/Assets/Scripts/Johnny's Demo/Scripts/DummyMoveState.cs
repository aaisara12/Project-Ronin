using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMoveState : StateMachineBehaviour
{
    [SerializeField]
    float maxAngularVelocity = 360;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 movement = Vector3.zero;

        movement += animator.GetBool("up") ? new Vector3(0, 0, 1) : Vector3.zero;
        movement -= animator.GetBool("down") ? new Vector3(0, 0, 1) : Vector3.zero;
        movement -= animator.GetBool("left") ? new Vector3(1, 0, 0) : Vector3.zero;
        movement += animator.GetBool("right") ? new Vector3(1, 0, 0) : Vector3.zero;

        if (movement.magnitude > 0)
        {
            movement.Normalize();

            animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, movement, Vector3.up), Vector3.up), maxAngularVelocity * Time.deltaTime);
        }

        animator.transform.position += movement * 3 * Time.deltaTime;
    }
}
