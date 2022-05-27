using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAudio : MonoBehaviour
{
    [SerializeField] HealthStat healthStat;
    [SerializeField] string damageSoundName;
    
    void Awake()
    {
        healthStat.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(HealthInfo obj)
    {
        AudioManager.instance.PlaySoundAtLocation(damageSoundName, transform.position);
    }
}
