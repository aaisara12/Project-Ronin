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

    public void OnAttack(InputAction.CallbackContext context){
        // var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        // Debug.Log(transform.position);
        // Debug.Log(Camera.main.ScreenToWorldPoint(pos));
        var playerLocation = Camera.main.WorldToScreenPoint(transform.position);
        var attackDirection = new Vector2(Input.mousePosition.x - playerLocation.x, Input.mousePosition.y - playerLocation.y);
        Debug.Log(attackDirection);
        MoveInDirection(attackDirection);
        animator.SetTrigger("attack");
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
    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        var playerLocation = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 v2 = new Vector2(playerLocation.x, playerLocation.y);
        var ray = new Vector2(Input.mousePosition.x - playerLocation.x, Input.mousePosition.y - playerLocation.y);
        // Gizmos.DrawRay(playerLocation, ray);
    }
}
