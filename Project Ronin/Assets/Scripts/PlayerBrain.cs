using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    private CharacterCaptureController captureController;
    void OnMove(InputValue movementVal){

        Vector3 movementVector = movementVal.Get<Vector3>();
        captureController.MoveInDirection(movementVector);

    }

    void OnDash(){
        captureController.DashInDirection(transform.forward);
    }

    void OnPause(){
        // Enter pause menu
    }
}
