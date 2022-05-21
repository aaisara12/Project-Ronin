using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIAssigner : MonoBehaviour
{
    [SerializeField] HealthUI healthUI;
    [SerializeField] HealthStat targetHealthStat;

    void Start()
    {
        healthUI.Initialize(targetHealthStat);
    }

}
