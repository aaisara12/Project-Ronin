using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredHealthStat : HealthStat
{
    public bool isArmorActivated = false;
    [SerializeField] int flatMitigation = 10;
    public void EnableArmor() => isArmorActivated = true;   // For animation event to modify armor
    public void DisableArmor() => isArmorActivated = false;

    public override void TakeDamage(int damage)
    {
        int armorFactor = isArmorActivated? 1 : 0;
        damage -= flatMitigation*armorFactor;
        if(damage < 0)
            damage = 0;
        
        if(isArmorActivated)
            AudioManager.instance.PlaySound("clink");
        base.TakeDamage(damage);
    }
}
