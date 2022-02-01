using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInput : MonoBehaviour
{
    AttributeSet attributeSet;

    // Start is called before the first frame update
    void Start()
    {
        attributeSet = GetComponent<AttributeSet>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            attributeSet.AddTag("up");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            attributeSet.RemoveTag("up");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attributeSet.AddTag("attack");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            attributeSet.RemoveTag("attack");
        }
    }
}
