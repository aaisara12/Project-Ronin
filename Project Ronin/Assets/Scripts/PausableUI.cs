using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableUI : MonoBehaviour
{
    public static event System.Action<bool> OnNewHasPausables;

    // List of UI objects requesting to pause the game
    private static List<Component> pauseQueue = new List<Component>();


    // We only allow UI scripts that intend to pause the game add themselves to this queue so that other
    // scripts don't mistakenly add themselves to it and leave the game in an awkward paused state as a result
    // of some non-UI script
    protected void RequestPause()
    {
        pauseQueue.Add(this);
        if(pauseQueue.Count == 1)
            OnNewHasPausables?.Invoke(true);
    }

    protected void RequestUnpause()
    {
        if(pauseQueue.Contains(this))
            pauseQueue.Remove(this);
        if(pauseQueue.Count == 0)
            OnNewHasPausables?.Invoke(false);

    }
}
