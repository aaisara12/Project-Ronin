using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StateMachineView : DialogueViewBase
{
    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler<string, float>("change_float", HandleChangeFloat);
        dialogueRunner.AddCommandHandler<string, float>("set_float", HandleSetFloat);
        dialogueRunner.AddCommandHandler<string>("add_tag", HandleTempTag);
        dialogueRunner.AddCommandHandler<string, float>("add_temp_tag", HandleAddTag);
        dialogueRunner.AddCommandHandler<string>("remove_tag", HandleRemoveTag);
    }

    // TODO: implement all those

    public override void DialogueStarted()
    {
        base.DialogueStarted();
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        base.RunLine(dialogueLine, onDialogueLineFinished);
    }

    public override void NodeComplete(string nextNode, Action onComplete)
    {
        base.NodeComplete(nextNode, onComplete);
    }

    private void HandleChangeFloat(string attr, float delta)
    {

    }

    private void HandleSetFloat(string attr, float newValue)
    {

    }

    private void HandleTempTag(string tag)
    {

    }

    private void HandleAddTag(string tag, float duration)
    {

    }

    private void HandleRemoveTag(string tag)
    {

    }
}
