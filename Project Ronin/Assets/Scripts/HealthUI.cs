using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    void Awake()
    {
        if(healthSlider == null)
            throw new System.Exception("HealthUI attached to \"" + gameObject.name + "\" does not have a slider assigned!");
    }

    public void HandleAttributeChanged()
    {
        // TODO: 
    }
}
