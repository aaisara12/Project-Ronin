using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    AttributeSet attr;

    private void Start()
    {
        attr = GetComponent<AttributeSet>();
    }

    public void OnAttributeChange()
    {
        if (attr.GetFloat("hp") <= 0)
        {
            Destroy(gameObject);
        }
    }
}
