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
        
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        PausableUI.OnNewHasPausables += SetPauseState;
    }

    void OnDisable()
    {
        PausableUI.OnNewHasPausables -= SetPauseState;
    }

    // We only want designated signals from the game to be able to trigger pauses to reduce the number of locations that
    // the pause state of the game can be changed (since it is globally readable)
    private void SetPauseState(bool shouldPause)
    {
        if(pauseState == PauseState.PAUSED && shouldPause == false)
        {
            Time.timeScale = 1;
            pauseState = PauseState.UNPAUSED;
            OnNewPauseState?.Invoke(pauseState);
        }
        if(pauseState == PauseState.UNPAUSED && shouldPause == true)
        {
            Time.timeScale = 0;
            pauseState = PauseState.PAUSED;
            OnNewPauseState?.Invoke(pauseState);
        }
            
    }

    
}

// State representing how "paused" the game is (in the future we may want certain aspects of the game to pause but not others)
public enum PauseState
{
    UNPAUSED,
    PAUSED
}
