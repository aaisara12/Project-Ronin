using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] AttributeSet targetAttributeSet;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            targetAttributeSet.ModifyFloat("hp", -5);
        }

    }

}
