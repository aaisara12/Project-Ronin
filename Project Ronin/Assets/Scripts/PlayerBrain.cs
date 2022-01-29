using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    private CharacterCaptureController captureController;
    [SerializeField] Interactor interactor;
    private Rigidbody rb;
    void Awake(){
        rb = GetComponent<Rigidbody>();
        captureController = GetComponent<CharacterCaptureController>();
    }
    void OnMove(InputValue movementVal){

        Vector2 inputVector = movementVal.Get<Vector2>();
        Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
        captureController?.MoveInDirection(movementVector);
        Debug.Log("Movement Vector:" + movementVector);
    }

    void OnDash(){
        Vector3 dashDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
        captureController?.DashInDirection(dashDirection);
        Debug.Log(dashDirection);
    }

    void OnPause(){
        // Enter pause menu
    }

    void OnInteract(){
        interactor.InteractWithNearestInRange();
    }
}
