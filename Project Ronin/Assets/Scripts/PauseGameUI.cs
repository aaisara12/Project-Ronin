using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class PauseGameUI : MonoBehaviour
{
    [SerializeField] private GameObject frame;
    public void TogglePauseMenu()
    {   
        if (frame.activeSelf)
        {
            frame.SetActive(false);
            UITracker.RemoveFromPauseQueue(this);
        }
        else
        {
            frame.SetActive(true);
            AudioManager.instance?.PlaySound("menu-open");
            UITracker.AddToPauseQueue(this);
        }
            
            
    }

    public void QuitGame()
    {
        Application.Quit();
        UITracker.RemoveFromPauseQueue(this);
    }
    
    public void DisplayUI()
    {
        Debug.Log("button was pressed");
    }
}
