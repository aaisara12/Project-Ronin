using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    private CharacterCaptureController captureController;
    [SerializeField] Interactor interactor;
    private Rigidbody rb;
    private Player_Controls playerControls;
    private void Awake(){
        rb = GetComponent<Rigidbody>();
        captureController = GetComponent<CharacterCaptureController>();
        playerControls = new Player_Controls();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }
    
    // private void OnMove(InputValue movementVal){

    //     Vector2 inputVector = movementVal.Get<Vector2>();
    //     Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
    //     captureController?.MoveInDirection(movementVector);
    //     // Debug.Log("Movement Vector:" + movementVector);
    // }

    private void OnDash(){
        Vector3 dashDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
        //captureController?.DashInDirection(dashDirection);
        Debug.Log(dashDirection);
    }

    private void OnPause(){
        // Enter pause menu
    }

    private void OnInteract(){
        interactor.InteractWithNearestInRange();
    }
    

    private void movePlayer(){
        Vector2 move = playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3(move.x, 0, move.y);
        captureController?.MoveInDirection(movementVector);
    }
    private void Update(){
        movePlayer();
    }
}
