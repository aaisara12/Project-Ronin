using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    // I think we should be careful about coupling our scripts so tightly
    // to auto-generated scripts since we might need to change them later and then
    // they would break a whole bunch of other code
    Player_Controls playerControls; 

    PauseGameUI pauseGameUI;
    void Awake()
    {
        // Depending on where the PauseGameUI is, we may need to find a new one when the scene loads?
        pauseGameUI = FindObjectOfType<PauseGameUI>();  // There should only be one (and we only do this once)
        playerControls = new Player_Controls();

        playerControls.Player.Pause.performed += HandlePausePerformed;
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void HandlePausePerformed(InputAction.CallbackContext context)
    {
        pauseGameUI?.TogglePauseMenu();
    }
}
