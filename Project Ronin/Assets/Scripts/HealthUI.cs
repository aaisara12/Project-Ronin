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

    void OnDisable()
    {
        if(this.attributeSet != null)
            this.attributeSet.DeregisterOnAttributeChange(this.handleHealthChange);
    }

    public void Initialize(AttributeSet attributeSet)
    {
        this.attributeSet = attributeSet;
        this.handleHealthChange = HandleHealthChange;
        healthSlider.maxValue = attributeSet.GetFloat(HEALTH_NAME);   // Assumes health on initialization is max health -- in the future it may be good to have a max value defined in the AttributeSet
        
        handleHealthChange.Invoke();

        this.attributeSet.RegisterOnAttributeChange(this.handleHealthChange);
    }

    void HandleHealthChange()
    {
        if(healthSlider == null)
            throw new System.Exception("HealthUI attached to \"" + gameObject.name + "\" does not have a slider assigned!");

        float newHealth = attributeSet.GetFloat(HEALTH_NAME);

        // Don't continue if no change has occurred -- it seems a little costly to call this every time ANY attribute has changed
        if(newHealth == healthSlider.value) {return;}    

        // Later on we may also want to trigger a coroutine for a lagging health bar (to show how much damage was dealt)
        Debug.Log("New health value: " + newHealth);
        healthSlider.value = newHealth;
    }
}
