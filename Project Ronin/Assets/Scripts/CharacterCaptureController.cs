using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCaptureController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 4f;
    Vector3 movementDirection;

    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    private float dashStartTime;
    private bool isDashing;
    private float dashSpeed;

    private Vector3[] angles = new Vector3[8];
    private Quaternion targetRotation;
    [SerializeField] private float rotateSpeed;

    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // D, U, R, L, DR, DL, UR, UL
        angles[0] = Quaternion.Euler(0, 45, 0) * transform.forward;
        angles[1] = Quaternion.Euler(0, 225, 0) * transform.forward;
        angles[2] = Quaternion.Euler(0, 135, 0) * transform.forward;
        angles[3] = Quaternion.Euler(0, -45, 0) * transform.forward;
        angles[4] = Quaternion.Euler(0, 0, 0) * transform.forward;
        angles[5] = Quaternion.Euler(0, 90, 0) * transform.forward;
        angles[6] = Quaternion.Euler(0, 270, 0) * transform.forward;
        angles[7] = Quaternion.Euler(0, 180, 0) * transform.forward;
    }

    void Update()
    {
        // if(Time.time > resetAttackSlowTime)
        //     attackSlowMultiplier = 1;

        // speed must be between 0-1
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);
        dashStartTime = 0;
        isDashing = false;
        dashSpeed = dashDistance / dashDuration;

        characterController.Move(movementDirection * movementSpeed * attackSlowMultiplier * Time.deltaTime);   
        
    }


    /// <summary>Request this character to move in direction of <paramref name = "directionVector"/></summary>
    public void MoveInDirection(Vector2 directionVector) 
    {
        if(isAttacking) {return;}
        //resetAttackSlowTime = 0;    // Instantly disrupt slow so player can move again as soon as they transition to move state

        // test with arrow keys
        if (directionVector == Vector2.zero || isDashing)
        {
            movementDirection = Vector3.zero;
            return;
        }

        if (directionVector.x == 0 && directionVector.y < 0)
        {
            targetRotation = Quaternion.LookRotation(angles[0]);
            movementDirection = angles[0];
        }
        else if (directionVector.x == 0 && directionVector.y > 0)
        {
            targetRotation = Quaternion.LookRotation(angles[1]);
            movementDirection = angles[1];
        }
        else if (directionVector.x < 0 && directionVector.y == 0)
        {
            targetRotation = Quaternion.LookRotation(angles[2]);
            movementDirection = angles[2];
        }
        else if (directionVector.x > 0 && directionVector.y == 0)
        {
            targetRotation = Quaternion.LookRotation(angles[3]);
            movementDirection = angles[3];
        }
        else if (directionVector.x > 0 && directionVector.y < 0)
        {
            targetRotation = Quaternion.LookRotation(angles[4]);
            movementDirection = angles[4];
        }
        else if (directionVector.x < 0 && directionVector.y < 0)
        {
            targetRotation = Quaternion.LookRotation(angles[5]);
            movementDirection = angles[5];
        }
        else if (directionVector.x > 0 && directionVector.y > 0)
        {
            targetRotation = Quaternion.LookRotation(angles[6]);
            movementDirection = angles[6];
        }
        else if (directionVector.x < 0 && directionVector.y > 0)
        {
            targetRotation = Quaternion.LookRotation(angles[7]);
            movementDirection = angles[7];
        }
    }

    /// <summary>Make this character perform a dash forwards</summary>
    public void DashForwards()
    {
        if (isDashing)
        {
            return;
        }
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        isDashing = true;
        Vector3 startPosition = transform.position;
        dashStartTime = 0;
        while (dashStartTime <= dashDuration && Vector3.Distance(startPosition, transform.position) <= dashDistance)
        {
            characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
            dashStartTime += Time.deltaTime;
            // Debug.Log(dashStartTime + "-" + Vector3.Distance(startPosition, transform.position)); // test moving set distance over set amt of time
            yield return null;
        }
        isDashing = false;
    }

    float attackSlowMultiplier = 1;
    bool isAttacking = false;
    public void AttackRotate(Vector2 attackDirection, MeleeState meleeState, bool shouldSlow = false)
    {
        isAttacking = true;
        // TODO: Smoothly rotate to the direction specified by attackDirection 
        // (interpret as directions with respect to World Space) for duration seconds (or until canceled by input command)

        targetRotation = Quaternion.LookRotation(new Vector3(attackDirection.x, 0, attackDirection.y), Vector3.up);
        attackSlowMultiplier = shouldSlow? 0.25f : 0f;
        meleeState.OnLeaveMelee += HandleLeaveMeleeState;   // Need to unsubscribe somewhere!!
        //resetAttackSlowTime = Time.time + duration;
    }

    void HandleLeaveMeleeState()
    {
        isAttacking = false;
        attackSlowMultiplier = 1;
    }
}
