using Demo.Enemy;
using Demo.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Chase")]
    public class ChaseAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
            var enemySightSensor = stateMachine.GetComponent<EnemySightSensor>();
            var animator = stateMachine.GetComponent<Animator>();

            // Debug.Log("CHASING!!!");
            animator.SetFloat("speed", 1f);
            navMeshAgent.SetDestination(enemySightSensor.Player.position);
        }
    }
}