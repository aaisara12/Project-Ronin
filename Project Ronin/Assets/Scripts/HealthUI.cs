using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;

    const string HEALTH_NAME = "hp";

    AttributeSet attributeSet;      // Initialize
    UnityAction handleHealthChange;

    float maxHealth;

    void OnDisable()
    {
        if(this.attributeSet != null)
            this.attributeSet.DeregisterOnAttributeChange(this.handleHealthChange);
    }

    public void Initialize(AttributeSet attributeSet)
    {
        this.attributeSet = attributeSet;
        this.handleHealthChange = HandleHealthChange;

        // It would be great if we could know what the max value of a specific attribute was (instead of having to store it here)
        maxHealth = attributeSet.GetFloat(HEALTH_NAME);   // Assumes health on initialization is max health -- in the future it may be good to have a max value defined in the AttributeSet
        
        handleHealthChange.Invoke();

        this.attributeSet.RegisterOnAttributeChange(this.handleHealthChange);
    }

    void HandleHealthChange()
    {
        if(healthSlider == null)
            throw new System.Exception("HealthUI attached to \"" + gameObject.name + "\" does not have a slider assigned!");

        float newHealth = attributeSet.GetFloat(HEALTH_NAME);  

        healthSlider.value = newHealth/maxHealth;
    }
}
