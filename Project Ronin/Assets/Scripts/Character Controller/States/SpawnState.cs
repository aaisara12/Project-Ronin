using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 1;
    }

}
