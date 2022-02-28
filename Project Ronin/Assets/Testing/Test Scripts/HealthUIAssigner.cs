using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIAssigner : MonoBehaviour
{
    [SerializeField] HealthUI healthUI;
    [SerializeField] AttributeSet targetAttributeSet;

    void Start()
    {
        healthUI.Initialize(targetAttributeSet);
    }

}
