using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacterActions : MonoBehaviour
{
    public void Attack()
    {
        Ability newAttck = AbilityPool.TakeAbility(AbilityPool.instance.dummyAttack, gameObject);
        newAttck.transform.position = transform.position;
    }

    public void Shoot()
    {
        Ability newShoot = AbilityPool.TakeAbility(AbilityPool.instance.dummyShoot, gameObject);
        newShoot.transform.position = transform.position + new Vector3(0, 0.5f, 0);
    }
}
