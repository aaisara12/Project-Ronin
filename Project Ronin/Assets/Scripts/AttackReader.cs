using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReader : MonoBehaviour
{
    Ability attackAbilityInstance;
    public void StartAttack()
    {
        attackAbilityInstance = AbilityPool.TakeAbility(AbilityPool.instance.attack, gameObject);
        AudioManager.instance?.PlaySound("swish-light");
    }
}
