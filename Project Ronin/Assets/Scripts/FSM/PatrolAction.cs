using Demo.Enemy;
using Demo.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Patrol")]
    public class PatrolAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
            var patrolPoints = stateMachine.GetComponent<PatrolPoints>();
            var animator = stateMachine.GetComponent<Animator>();

            animator.SetFloat("speed", 0.5f);
            // animator.SetFloat("xInput", 10f);
            // Debug.Log("Patrolling");
            if (patrolPoints.HasReached(navMeshAgent))
                navMeshAgent.SetDestination(patrolPoints.GetNext().position);
        }
    }
}