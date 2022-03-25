using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public event System.Action OnPauseGame;
    public event System.Action<bool> OnNewPauseState;   // More compact pause event (use in future sprints)

    public static GameManager Instance {get; private set;}

    bool isPaused = false;

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

    /// <summary> Freezes all game objects in scene </summary>
    public void PauseGame()
    {
        SetPauseState(true);
    }

    // This is a more flexible public method for setting pause state
    // that will be transitioned to in a future sprint
    public void SetPauseState(bool shouldPause)
    {
        // if(!isPaused && shouldPause)
        // {
        //     isPaused = true;
        //     OnPauseGame?.Invoke();
        //     OnNewPauseState?.Invoke(isPaused);
        // }
        // else if(isPaused && !shouldPause)
        // {
        //     isPaused = false;
        //     OnNewPauseState?.Invoke(isPaused);
        // }

        Time.timeScale = shouldPause? 0 : 1;
        isPaused = shouldPause;
    }

    // Potentially useful function if the pause state is not known and thus cannot call SetPauseState properly
    public void TogglePause()
    {
        SetPauseState(!isPaused);
    }
}
