using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn.Unity;

namespace Obselete
{
    public class TransitionProfile
    {
        // used by state machine
        public AttributeSet attributes { private get; set; }
        public bool closed = false;

        Dictionary<string, int> optionMapping = new Dictionary<string, int>();
        Dictionary<string, Condition> activeTransitions = new Dictionary<string, Condition>();

        string currentDestination = "";
        Stack<Condition> conditionBuilder = new Stack<Condition>();

        public int CheckTransition()
        {
            if (closed)
            {
                foreach (var transition in activeTransitions)
                {
                    if (transition.Value.Evaluate())
                    {
                        return optionMapping[transition.Key];
                    }
                }
            }

            throw new AccessViolationException("TransitionProfile object evaluated without finishing setup.");
        }

        public void RegisterOptions(DialogueOption[] options)
        {
            foreach (var option in options)
            {
                optionMapping.Add(option.Line.RawText, option.DialogueOptionID);
            }
        }

        public void HandleAddTransitionOpen(string destination)
        {
            currentDestination = destination;

            // logically this line cleans up any state from the previous transition
            conditionBuilder.Clear();
        }

        public void HandleClose()
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
                AndClause newTransition = new AndClause();
                newTransition.Include(closedConditions);
                activeTransitions.Add(currentDestination, newTransition);
                // closed = true;
            }
        }

        public void HandleAndOpen()
        {
            conditionBuilder.Push(new AndClause());
        }

        public void HandleOrOpen()
        {
            conditionBuilder.Push(new OrClause());
        }

        public void HandleFloatCheck(string attrName, string op, float refValue)
        {
            SimpleCondition newCondition;
            switch (op)
            {
                case ">":
                    newCondition = new SimpleCondition(() => { return attributes.GetFloat(attrName) > refValue; });
                    break;
                case "<":
                    newCondition = new SimpleCondition(() => { return attributes.GetFloat(attrName) < refValue; });
                    break;
                case "==":
                    newCondition = new SimpleCondition(() => { return attributes.GetFloat(attrName) == refValue; });
                    break;
                case ">=":
                    newCondition = new SimpleCondition(() => { return attributes.GetFloat(attrName) >= refValue; });
                    break;
                case "<=":
                    newCondition = new SimpleCondition(() => { return attributes.GetFloat(attrName) <= refValue; });
                    break;
                default:
                    newCondition = new SimpleCondition(() => { return false; });
                    break;
            }

            conditionBuilder.Push(newCondition);
        }

        public void HandleHasTagCheck(string tagName, bool inclusive)
        {
            conditionBuilder.Push(new SimpleCondition(() =>
            {
                bool result = attributes.CheckTag(tagName);
                return inclusive ? result : !result;
            }));
        }

        // helper classes
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
    }
}