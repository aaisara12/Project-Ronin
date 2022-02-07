using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    private CharacterCaptureController character;
    // Start is called before the first frame update
    void Start()
    {
        character = FindObjectOfType<CharacterCaptureController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right") && Input.GetKey("up"))
        {
            character.MoveInDirection(new Vector2(2,2));
        }

        else if (Input.GetKey("left") && Input.GetKey("up"))
        {
            character.MoveInDirection(new Vector2(-2,2));
        }

        else if (Input.GetKey("right") && Input.GetKey("down"))
        {
            character.MoveInDirection(new Vector2(2,-2));
        }

        else if (Input.GetKey("left") && Input.GetKey("down"))
        {
            character.MoveInDirection(new Vector2(-2,-2));
        }

        else if (Input.GetKey("right"))
        {
            character.MoveInDirection(new Vector2(2,0));
        }

        else if (Input.GetKey("up"))
        {
            character.MoveInDirection(new Vector2(0,2));
        }

        else if (Input.GetKey("down"))
        {
            character.MoveInDirection(new Vector2(0,-2));
        }

        else if (Input.GetKey("left"))
        {
            character.MoveInDirection(new Vector2(-2,0));
        }

        else 
        {
            character.MoveInDirection(new Vector2(0,0));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            character.DashForwards(new Vector2(0,0));
        }
    }
}
