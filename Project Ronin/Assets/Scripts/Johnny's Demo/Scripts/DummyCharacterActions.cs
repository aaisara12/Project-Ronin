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
}
