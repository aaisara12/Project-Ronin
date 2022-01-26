using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// the best documentation is naming

public class StateMachineView : DialogueViewBase
{
    [SerializeField]
    DialogueRunner dialogueRunner = null;
    [SerializeField]
    Attributes attributes = null;

    IStateTable characterStateTable = null;

    // condition objects used for evaluation
    private abstract class Condition
    {
        public bool done = false;
        public abstract bool Evaluate();
        public virtual void Include(List<Condition> includee)
        {
            throw new NotImplementedException("Include called upon simple condition!");
        }
    }

    private class SimpleCondition : Condition
    {
        Func<bool> condition;

        public SimpleCondition(Func<bool> newCondition)
        {
            done = true;
            condition = newCondition;
        }

        public override bool Evaluate()
        {
            return condition();
        }
    }

    private class AndClause : Condition
    {
        public List<Condition> conditions = new List<Condition>();

        public override bool Evaluate()
        {
            bool result = true;

            foreach (Condition condition in conditions)
            {
                result &= condition.Evaluate();
            }

            return result;
        }

        public override void Include(List<Condition> includee)
        {
            conditions.AddRange(includee);
        }
    }

    private class OrClause : Condition
    {
        public List<Condition> conditions = new List<Condition>();

        public override bool Evaluate()
        {
            bool result = true;

            foreach (Condition condition in conditions)
            {
                result |= condition.Evaluate();
            }

            return result;
        }

        public override void Include(List<Condition> includee)
        {
            conditions.AddRange(includee);
        }
    }

    // active transitions
    List<Condition> activeTransitions = new List<Condition>();
    Action<int> currentCallback = null;
    Stack<Condition> conditionBuilder = new Stack<Condition>();

    // state
    ICharacterState currentState;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("transition", HandleAddTransitionOpen);
        dialogueRunner.AddCommandHandler("/", HandleClose);
        dialogueRunner.AddCommandHandler("and", HandleAndOpen);
        dialogueRunner.AddCommandHandler("or", HandleOrOpen);
    }

    private void Start()
    {
        characterStateTable = GetComponent<IStateTable>();
        if (!dialogueRunner) dialogueRunner = GetComponent<DialogueRunner>();
        if (!attributes) attributes = GetComponent<Attributes>();

        dialogueRunner.StartDialogue("Entry");
    }

    private void Update()
    {
        currentState?.Update();

        for (int i = 0; i < activeTransitions.Count; i++)
        {
            if (activeTransitions[i].Evaluate())
            {
                currentCallback(i); // TODO: test if this starts at 1 or 0
            }
        }
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        currentCallback = onOptionSelected;
    }

    public override void NodeComplete(string nextNode, Action onComplete)
    {
        // load next state (node name is the key to states)
        currentState.OnExit();
        currentState = characterStateTable.GetState(nextNode);
        currentState.OnEnter();

        base.NodeComplete(nextNode, onComplete);
    }

    // handlers
    private void HandleAddTransitionOpen()
    {
        // seems there's nothing to do
        // logically this line cleans up any state from the previous transition
        conditionBuilder.Clear();
    }

    private void HandleClose()
    {
        List<Condition> closedConditions = new List<Condition>();

        while (conditionBuilder.Count > 0 && conditionBuilder.Peek().done)
        {
            closedConditions.Add(conditionBuilder.Pop());
        }

        if (conditionBuilder.Count > 0) // closing a clause
        {
            conditionBuilder.Peek().Include(closedConditions);
            conditionBuilder.Peek().done = true;
        }
        else // closing a transition
        {
            if (closedConditions.Count > 1)
            {
                activeTransitions.AddRange(closedConditions);
            }
            else
            {
                AndClause newTransition = new AndClause();
                newTransition.Include(closedConditions);
                activeTransitions.Add(newTransition);
            }
        }
    }

    private void HandleAndOpen()
    {
        conditionBuilder.Push(new AndClause());
    }

    private void HandleOrOpen()
    {
        conditionBuilder.Push(new OrClause());
    }

    private void HandleFloatCheck(string attrName, string op, float refValue)
    {
        SimpleCondition newCondition;
        float value = attributes.GetFloat(attrName);
        switch (op)
        {
            case ">":
                newCondition = new SimpleCondition(() => { return value > refValue; });
                break;
            case "<":
                newCondition = new SimpleCondition(() => { return value < refValue; });
                break;
            case "==":
                newCondition = new SimpleCondition(() => { return value == refValue; });
                break;
            case ">=":
                newCondition = new SimpleCondition(() => { return value >= refValue; });
                break;
            case "<=":
                newCondition = new SimpleCondition(() => { return value <= refValue; });
                break;
            default:
                newCondition = new SimpleCondition(() => { return false; });
                break;
        }

        conditionBuilder.Push(newCondition);
    }

    private void HandleHasTagCheck(string tagName, bool inclusive)
    {
        conditionBuilder.Push(new SimpleCondition(() =>
        {
            bool result = attributes.CheckTag(tagName);
            return inclusive ? result : !result;
        }));
    }
}
