using UnityEngine;
using System.Collections.Generic;
using System;

public class DummyCharacterFSM : MonoBehaviour
{
    State currentState = null;
    public Animator animator;
    AttributeSet attributeSet;

    List<Action> deferredActions = new List<Action>();

    private void Start()
    {
        currentState = new IdleState();
        currentState.fsm = this;
        currentState.Start();

        // animator = GetComponent<Animator>();
        attributeSet = GetComponent<AttributeSet>();
    }

    private void Update()
    {
        foreach (Action action in deferredActions)
        {
            action.Invoke();
        }
        deferredActions.Clear();

        currentState.Update();
    }

    private void LateUpdate()
    {
        currentState.LateUpdate();

        State tempNextState = currentState.NextState();
        if (tempNextState != currentState)
        {
            currentState.OnExit();

            currentState = tempNextState;
            currentState.fsm = this;

            currentState.OnEnter();
            deferredActions.Add(() => { currentState.Start(); });

            Debug.Log(currentState);
        }
    }

    private abstract class State
    {
        public DummyCharacterFSM fsm;

        public abstract State NextState();
        public virtual void OnEnter() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void OnExit() { }
    }

    private class IdleState : State
    {
        public override State NextState()
        {
            if (fsm.attributeSet.CheckTag("up"))
            {
                return new MoveState();
            }
            else if (fsm.attributeSet.CheckTag("attack"))
            {
                return new AttackState();
            }
            else { return this; }
        }
    }

    private class MoveState : State
    {
        public override State NextState()
        {
            if (fsm.attributeSet.CheckTag("attack"))
            {
                return new AttackState();
            }
            else if (!fsm.attributeSet.CheckTag("up"))
            {
                return new IdleState();
            }
            else { return this; }
        }

        public override void OnEnter()
        {
            fsm.animator.SetBool("Moving", true);
        }

        public override void OnExit()
        {
            fsm.animator.SetBool("Moving", false);
        }

        public override void Update()
        {
            fsm.transform.position += new Vector3(0, 0, 2 * Time.deltaTime);
        }
    }

    private class AttackState : State
    {
        bool applied = false;

        public override State NextState()
        {
            if (applied && fsm.animator.IsInTransition(0))
            {
                if (fsm.attributeSet.CheckTag("up"))
                    return new MoveState();
                else
                    return new IdleState();
            }
            else
            {
                return this;
            }
        }

        public override void OnEnter()
        {
            fsm.animator.SetTrigger("Attack");
        }

        //public override void Start()
        //{
        //    base.Start();
        //}

        public override void LateUpdate()
        {
            if (!applied && !fsm.animator.IsInTransition(0))
            {
                applied = true;
                Debug.Log("apply attack");
                var attack = AbilityPool.TakeAbility(AbilityPool.instance.dummyAttack);
                attack.user = fsm.attributeSet;
                attack.transform.position = fsm.transform.position;
                attack.transform.rotation = fsm.transform.rotation;
            }
        }
    }
}
