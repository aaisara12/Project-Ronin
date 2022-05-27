using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTimer : MonoBehaviour
{
    // MonoBehaviour for keeping track of attack cooldown state for enemies
    [SerializeField] float mainCooldown = 5;
    [SerializeField] float specialCooldown = 20;
    [SerializeField] bool specialEnabled = false;

    float mainLastUsed = 0;
    float specialLastUsed = 0;

    public bool TryAttack()
    {
        if(mainLastUsed + mainCooldown < Time.time)
        {
            mainLastUsed = Time.time;
            return true;
        }
        return false;
    }

    public bool TrySummon()
    {
        if(specialEnabled && specialLastUsed + specialCooldown < Time.time)
        {
            specialLastUsed = Time.time;
            return true;
        }
        return false;
    }
}
