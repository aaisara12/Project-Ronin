using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCaptureController : MonoBehaviour
{
    public float movementSpeed = 4f;
    Vector3 forward;
    Vector3 right;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    private float dashStartTime;
    private bool isDashing;
    private float dashSpeed;

    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        dashStartTime = 0;
        isDashing = false;
        dashSpeed = dashDistance / dashDuration;
    }

    /// <summary>Request this character to move in direction of <paramref name = "directionVector"/></summary>
    public void MoveInDirection(Vector3 directionVector)
    {
        MoveInDirection(new Vector2(directionVector.x, directionVector.z));
    }

    /// <summary>Make this character perform a dash in direction of <paramref name = "directionVector"/></summary>
    public void DashInDirection(Vector3 directionVector)
    {
        // test with shift key
        StartCoroutine(Dash());
    }

    public void MoveInDirection(Vector2 directionVector) 
    {
        // test with arrow keys
        if (directionVector == Vector2.zero || isDashing)
        {
            // do nothing
            return;
        }
        directionVector = directionVector.normalized;
        Vector3 rightMovement = right * movementSpeed * Time.deltaTime * directionVector.x;
        Vector3 upMovement = forward * movementSpeed * Time.deltaTime * directionVector.y;
        rightMovement = Vector3.ClampMagnitude(rightMovement, 1);
        upMovement = Vector3.ClampMagnitude(upMovement, 1);

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        characterController.Move(transform.forward * movementSpeed * Time.deltaTime);
    }

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
}