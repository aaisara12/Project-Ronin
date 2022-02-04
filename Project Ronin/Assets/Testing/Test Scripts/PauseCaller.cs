using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseCaller : MonoBehaviour
{
    Player_Controls playerControls;

    void Awake()
    {
        playerControls = new Player_Controls();
        playerControls.Enable();
    }

    void OnEnable()
    {
        playerControls.Player.Pause.performed += HandlePauseKeyPress;
    }

    void HandlePauseKeyPress(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause();
    }

}
