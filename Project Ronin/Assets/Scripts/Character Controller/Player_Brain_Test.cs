using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Brain_Test : MonoBehaviour
{
    private CharacterCaptureController captureController;
    // [SerializeField] Interactor interactor;
    private Rigidbody rb;
    private Player_Controls playerControls;
    public Animator animator;
    private void Awake(){
        rb = GetComponent<Rigidbody>();
        captureController = GetComponent<CharacterCaptureController>();
        playerControls = new Player_Controls();
    }

    private void OnEnable(){
        playerControls.Enable();

        playerControls.Player.Aim.performed += Aim;
        playerControls.Player.Aim.canceled += cancelAim;
        playerControls.Player.Move.performed += movePlayer;
        playerControls.Player.Move.canceled += stopPlayer;
    }

    private void OnDisable(){
        playerControls.Disable();

        playerControls.Player.Aim.performed -= Aim;
        playerControls.Player.Aim.canceled -= cancelAim;
        playerControls.Player.Move.performed -= movePlayer;
        playerControls.Player.Move.canceled -= stopPlayer;
    }

    private void Aim(InputAction.CallbackContext context){
        animator.SetBool("aim", true);
    }

    private void cancelAim(InputAction.CallbackContext context){
        animator.SetBool("aim", false);
    }

    private void OnPause(){
        // Enter pause menu
    }

    private void OnAttack(){
        animator.SetTrigger("attack");
    }

    private void OnParry(){
        animator.SetTrigger("parry");
    }

    private void OnCharge(){
        animator.SetTrigger("charge");
    }

    private void OnShock(){
        animator.SetTrigger("shock");
    }

    private void OnDodge(){
        animator.SetTrigger("dodge");
    }

    private void movePlayer(InputAction.CallbackContext context){
        Vector2 move = context.ReadValue<Vector2>();
        animator.SetFloat("xInput", move.x);
        animator.SetFloat("yInput", move.y);
    }
    private void stopPlayer(InputAction.CallbackContext context){
        animator.SetFloat("xInput", 0);
        animator.SetFloat("yInput", 0);
    }
}
