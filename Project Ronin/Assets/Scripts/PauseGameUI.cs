using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class PauseGameUI : PausableUI
{
    [SerializeField] private GameObject frame;
    public void TogglePauseMenu()
    {   
        if (frame.activeSelf)
        {
            frame.SetActive(false);
            RequestUnpause();
        }
        else
        {
            frame.SetActive(true);
            AudioManager.instance?.PlaySound("menu-open");
            RequestPause();
        }
            
            
    }

    public void QuitGame()
    {
        Application.Quit();
        RequestUnpause();
    }
    
    public void DisplayUI()
    {
        Debug.Log("button was pressed");
    }
}
