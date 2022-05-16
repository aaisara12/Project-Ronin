using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    private CharacterCaptureController captureController;
    // [SerializeField] Interactor interactor;
    private Rigidbody rb;
    private Player_Controls playerControls;
    public Animator animator;

    [SerializeField] AttackInputProxy attackInputProxy;



    private void Awake(){
        rb = GetComponent<Rigidbody>();
        captureController = GetComponent<CharacterCaptureController>();
        playerControls = new Player_Controls();
    }

    void Start()
    {
        // In start since GameManager instance created in Awake at some arbitrary point in time
        GameManager.Instance.OnNewPauseState += HandleNewPauseState;
    }


    // Prevent the player from doing any actions while the game is paused
    private void HandleNewPauseState(PauseState pauseState)
    {
        if(pauseState == PauseState.PAUSED)
            playerControls.Disable();
        else
            playerControls.Enable();
    }

    private void OnEnable(){
        playerControls.Enable();

        playerControls.Player.Aim.performed += Aim;
        playerControls.Player.Aim.canceled += cancelAim;
        playerControls.Player.Move.performed += movePlayer;
        playerControls.Player.Move.canceled += stopPlayer;

        playerControls.Player.Attack.performed += OnAttack;
        playerControls.Player.Parry.performed += OnParry;
        playerControls.Player.Shock.performed += OnShock;
        playerControls.Player.Dodge.performed += OnDodge;
        playerControls.Player.Charge.performed += OnCharge;
    }

    private void OnDisable(){
        playerControls.Disable();

        playerControls.Player.Aim.performed -= Aim;
        playerControls.Player.Aim.canceled -= cancelAim;
        playerControls.Player.Move.performed -= movePlayer;
        playerControls.Player.Move.canceled -= stopPlayer;

        playerControls.Player.Attack.performed -= OnAttack;
        playerControls.Player.Parry.performed -= OnParry;
        playerControls.Player.Shock.performed -= OnShock;
        playerControls.Player.Dodge.performed -= OnDodge;
        playerControls.Player.Charge.performed -= OnCharge;

        GameManager.Instance.OnNewPauseState -= HandleNewPauseState;
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

    private void OnAttack(InputAction.CallbackContext context){

        Vector2 playerLocation = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 attackDirection = new Vector2(Input.mousePosition.x - playerLocation.x, Input.mousePosition.y - playerLocation.y);
        double sqrt2 = Math.Sqrt(2);
        double newX = ((attackDirection.x * sqrt2) + (attackDirection.y * sqrt2)) / 2;
        double newY = (-1 * (attackDirection.x * sqrt2) + (attackDirection.y * sqrt2)) / 2;
        Vector2 rotatedAttackDirection = new Vector2((float) newX, (float) newY);

        animator.SetFloat("xAttack", rotatedAttackDirection.x);
        animator.SetFloat("yAttack", rotatedAttackDirection.y);
        
        attackInputProxy.RequestAttack();
    }

    private void OnParry(InputAction.CallbackContext context){
        animator.SetTrigger("parry");
    }

    private void OnCharge(InputAction.CallbackContext context){
        animator.SetTrigger("charge");
    }

    private void OnShock(InputAction.CallbackContext context){
        animator.SetTrigger("shock");
    }

    private void OnDodge(InputAction.CallbackContext context){
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
