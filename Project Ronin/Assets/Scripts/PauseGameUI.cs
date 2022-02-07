using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class PauseGameUI : MonoBehaviour
{
    [SerializeField] private GameObject frame;
    public void TogglePauseMenu()
    {
        GameManager.Instance.TogglePause();
        if (frame.activeSelf)
            frame.SetActive(false);
        else
            frame.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void DisplayUI()
    {
        Debug.Log("button was pressed");
    }
}
