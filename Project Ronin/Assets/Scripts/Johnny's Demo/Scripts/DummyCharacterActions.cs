using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacterActions : MonoBehaviour
{
    public void Attack()
    {
        Ability newAttck = AbilityPool.TakeAbility(AbilityPool.instance.dummyAttack, gameObject);
    }

    public void Shoot()
    {
        Ability newShoot = AbilityPool.TakeAbility(AbilityPool.instance.dummyShoot, gameObject);
    }
}
