using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;

    IHealthStat healthStat;

    float maxHealth;

    void OnDisable()
    {
        if(this.healthStat != null)
            this.healthStat.OnHealthChanged -= HandleHealthChange;
    }

    public void Initialize(IHealthStat healthStat)
    {
        this.healthStat = healthStat;
        HandleHealthChange(new HealthInfo {current = healthStat.health, max = healthStat.maxHealth});

        this.healthStat.OnHealthChanged += HandleHealthChange;
    }

    void HandleHealthChange(HealthInfo info)
    {
        if(healthSlider == null)
            throw new System.Exception("HealthUI attached to \"" + gameObject.name + "\" does not have a slider assigned!");

        healthSlider.value = info.current/info.max;
    }
}
