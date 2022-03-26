using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Special class for keeping track of the state of the UI elements in the scene
public static class UITracker
{
    public static event System.Action<QueueState> OnNewPauseQueueState;

    // List of UI elements that have requested to pause the game
    private static List<Object> pauseQueue = new List<Object>();

    public static void AddToPauseQueue(Object uiComponent)
    {
        pauseQueue.Add(uiComponent);
        if(pauseQueue.Count == 1)
            OnNewPauseQueueState?.Invoke(QueueState.NON_EMPTY);
    }

    public static void RemoveFromPauseQueue(Object uiComponent)
    {
        if(pauseQueue.Contains(uiComponent))
            pauseQueue.Remove(uiComponent);
        if(pauseQueue.Count == 0)
            OnNewPauseQueueState?.Invoke(QueueState.EMPTY);

    }
}

public enum QueueState
{
    EMPTY,
    NON_EMPTY
}
