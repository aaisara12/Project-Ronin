using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PauseState pauseState;

    public event System.Action<PauseState> OnNewPauseState;   // More compact pause event (use in future sprints)

    public static GameManager Instance {get; private set;}


    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        UITracker.OnNewPauseQueueState += HandleNewPauseQueueState;
        
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        UITracker.OnNewPauseQueueState -= HandleNewPauseQueueState;
    }

    private void HandleNewPauseQueueState(QueueState queueState)
    {
        // Have a wrapper around SetPauseState instead of making SetPauseState directly take a QueueState parameter in case
        // we want to use SetPauseState in a different situation (keeping it decoupled from queue state)
        SetPauseState(queueState == QueueState.NON_EMPTY);
    }

    // We only want designated signals from the game to be able to trigger pauses to reduce the number of locations that
    // the pause state of the game can be changed (since it is globally readable)
    private void SetPauseState(bool shouldPause)
    {
        pauseState = shouldPause? PauseState.PAUSED : PauseState.UNPAUSED;
        Time.timeScale = (pauseState == PauseState.PAUSED)? 0 : 1;
        OnNewPauseState?.Invoke(pauseState);
    }
}

// State representing how "paused" the game is (in the future we may want certain aspects of the game to pause but not others)
public enum PauseState
{
    UNPAUSED,
    PAUSED
}
