using Demo.FSM;
using UnityEngine;
namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/Perform Special")]
    public class PerformSpecialDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
           return stateMachine.GetComponent<AttackTimer>().TrySummon();
        }
    }
}