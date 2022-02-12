using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCaptureController : MonoBehaviour
{
    public float movementSpeed = 4f;
    Vector3 forward;
    Vector3 right;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
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

    IEnumerator Dash()
    {
        rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero;
    }

    public void MoveInDirection(Vector2 directionVector) 
    {
        // test with arrow keys
        if (directionVector == Vector2.zero)
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
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    public void DashForwards()
    {
        StartCoroutine(Dash());
    }
}