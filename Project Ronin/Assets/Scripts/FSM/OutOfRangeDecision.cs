using Demo.Enemy;
using Demo.FSM;
using UnityEngine;
using UnityEngine.AI;
namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/Out Of Range")]
    public class OutOfRangeDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var Player = GameObject.Find("Ronin").transform;
            var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
            if(Vector3.Distance(navMeshAgent.transform.position, Player.position) > 2f){
                return true;
            }
            else{
                return false;
            }
        }
    }
}