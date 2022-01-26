using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StateMachineView : DialogueViewBase
{
    public DialogueRunner dialogueRunner;

    // active transitions
    // TODO: implement transition class

    private void Awake()
    {
        //dialogueRunner.AddCommandHandler<string, float>("change_float", HandleChangeFloat);
        //dialogueRunner.AddCommandHandler<string, float>("set_float", HandleSetFloat);
        //dialogueRunner.AddCommandHandler<string>("add_tag", HandleTempTag);
        //dialogueRunner.AddCommandHandler<string, float>("add_temp_tag", HandleAddTag);
        //dialogueRunner.AddCommandHandler<string>("remove_tag", HandleRemoveTag);
    }

    private void Start()
    {
        // TODO: initialte runner
    }

    // TODO: implement all those

    public override void DialogueStarted()
    {
        base.DialogueStarted();
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        // TODO: register callback
    }


    // condition objects used for evaluation
    private interface Condition
    {
        bool Evaluate();
    }

    private class SimpleCondition : Condition
    {
        Func<bool> condition;

        public bool Evaluate()
        {
            return condition();
        }
    }
    
    private class AndClause : Condition
    {
        public List<Condition> conditions = new List<Condition>();

        public bool Evaluate()
        {
            bool result = true;

            foreach (Condition condition in conditions)
            {
                result &= condition.Evaluate();
            }

            return result;
        }
    }

    private class OrClause : Condition
    {
        public List<Condition> conditions = new List<Condition>();

        public bool Evaluate()
        {
            bool result = true;

            foreach (Condition condition in conditions)
            {
                result |= condition.Evaluate();
            }

            return result;
        }
    }

    // handlers building the 
    private void HandleSetState(string nodeName)
    {
        throw new NotImplementedException();
    }

    private void HandleAddTransition()
    {
        throw new NotImplementedException();
    }

    private void HandleAndOpen()
    {
        throw new NotImplementedException();
    }

    private void HandleAndClose()
    {
        throw new NotImplementedException();
    }

    private void HandleOrOpen()
    {
        throw new NotImplementedException();
    }

    private void HandleOrClose()
    {
        throw new NotImplementedException();
    }
}
