using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryReader : MonoBehaviour
{
    private AttributeSet attributeSet;
    // Start is called before the first frame update
    void Start()
    {
        attributeSet = AttributeSet.objectToAttributes[gameObject];
    }

    // Update is called once per frame
    public void toggleParryOn()
    {
        attributeSet.AddTag("isParrying");
        Debug.Log(attributeSet.CheckTag("isParrying"));
    }

    public void toggleParryOff()
    {
        attributeSet.RemoveTag("isParrying");
        Debug.Log(attributeSet.CheckTag("isParrying"));
    }
}
