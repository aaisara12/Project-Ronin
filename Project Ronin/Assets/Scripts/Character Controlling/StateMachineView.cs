using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// the best documentation is naming

public class StateMachineView : DialogueViewBase
{
    public DialogueRunner dialogueRunner;

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

        SimpleCondition() { done = true; }

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

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("transition", HandleAddTransitionOpen);
        dialogueRunner.AddCommandHandler("/", HandleClose);
        dialogueRunner.AddCommandHandler("and", HandleAndOpen);
        dialogueRunner.AddCommandHandler("or", HandleOrOpen);
    }

    private void Start()
    {
        // TODO: initialize runner
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        currentCallback = onOptionSelected;
    }

    public override void NodeComplete(string nextNode, Action onComplete)
    {
        // TODO: load next state (node name is the key to states)
        base.NodeComplete(nextNode, onComplete);
    }

    // handlers building the -
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
}
