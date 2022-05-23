using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTimer : MonoBehaviour
{
    // MonoBehaviour for keeping track of attack cooldown state for enemies
    [SerializeField] float cooldown = 5;

    float lastUsed = 0;

    public bool TryAttack()
    {
        if(lastUsed + cooldown < Time.time)
        {
            lastUsed = Time.time;
            return true;
        }
        return false;
    }
}
