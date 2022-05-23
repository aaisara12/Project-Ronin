using Demo.Enemy;
using Demo.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Attack")]
    public class AttackAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            var animator = stateMachine.GetComponent<Animator>();
            
            var at = stateMachine.GetComponent<AttackTimer>();

            if(at.TryAttack())
            {
                animator.SetTrigger("attack");
            }
            
        }
    }
}