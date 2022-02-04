using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnPauseGame += DisplayUI;
    }

    void OnDisable()
    {
        GameManager.Instance.OnPauseGame -= DisplayUI;
    }

    public void DisplayUI()
    {
        Debug.Log("button was pressed");
    }
}
