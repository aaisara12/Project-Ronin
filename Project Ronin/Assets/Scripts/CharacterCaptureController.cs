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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 nothing = new Vector3(0, 0, 0);
            DashInDirection(nothing);
        }
        Vector3 dir;
        if (Input.GetKey("up"))
        {
            if (Input.GetKey("left"))
            {
                dir = new Vector3(-1, 0, 1);
            }
            else if (Input.GetKey("right"))
            {
                dir = new Vector3(1, 0, 1);
            }
            else
            {
                dir = new Vector3(0, 0, 1);
            }
        }
        else if (Input.GetKey("down"))
        {
           if (Input.GetKey("left"))
            {
                dir = new Vector3(-1, 0, -1);
            }
            else if (Input.GetKey("right"))
            {
                dir = new Vector3(1, 0, -1);
            }
            else
            {
                dir = new Vector3(0, 0, -1);
            }
        }
        else if (Input.GetKey("left"))
        {
            if (Input.GetKey("up"))
            {
                dir = new Vector3(-1, 0, 1);
            }
            else if (Input.GetKey("down"))
            {
                dir = new Vector3(-1, 0, -1);
            }
            else
            {
                dir = new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKey("right"))
        {
            if (Input.GetKey("up"))
            {
                dir = new Vector3(1, 0, 1);
            }
            else if (Input.GetKey("down"))
            {
                dir = new Vector3(1, 0, -1);
            }
            else
            {
                dir = new Vector3(1, 0, 0);
            }
        }
        else
        {
            dir = new Vector3(0, 0, 0);
        }
        MoveInDirection(dir);
    }

    /// <summary>Request this character to move in direction of <paramref name = "directionVector"/></summary>
    public void MoveInDirection(Vector3 directionVector)
    {
        // test with arrow keys
        if (directionVector == Vector3.zero)
        {
            // do nothing
            return;
        }
        directionVector = Vector3.Normalize(directionVector);
        Vector3 rightMovement = right * movementSpeed * Time.deltaTime * directionVector.x;
        Vector3 upMovement = forward * movementSpeed * Time.deltaTime * directionVector.z;
        rightMovement = Vector3.ClampMagnitude(rightMovement, 1);
        upMovement = Vector3.ClampMagnitude(upMovement, 1);

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
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
}
