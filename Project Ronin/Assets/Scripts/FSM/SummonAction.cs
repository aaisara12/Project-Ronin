using Demo.Enemy;
using Demo.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Summon")]
    public class SummonAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            var animator = stateMachine.GetComponent<Animator>();
        

            animator.SetTrigger("summon");
            
        }
    }
}