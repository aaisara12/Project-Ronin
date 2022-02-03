using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInput : MonoBehaviour
{
    AttributeSet attributeSet;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        attributeSet = GetComponent<AttributeSet>();   
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("up", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("down", true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("left", true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("right", true);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("up", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("down", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("left", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("right", false);
        }

        animator.SetBool("moving", isMoving);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("shoot");
        }
    }
}
