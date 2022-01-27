using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] CharacterCaptureController captureController;
    private Rigidbody rb;
    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    void OnMove(InputValue movementVal){

        Vector2 movementVector = movementVal.Get<Vector2>();
        // captureController.MoveInDirection(movementVector);
        // Debug.Log("Movement Vector:" + movementVector);
    }

    void OnDash(){
        Vector2 dashDirection = new Vector2(transform.forward.x, transform.forward.y);
        // captureController.DashInDirection(dashDirection);
        // Debug.Log(dashDirection);
    }

    void OnPause(){
        // Enter pause menu
    }
}
