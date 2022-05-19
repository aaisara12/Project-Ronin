using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeMove : RemoteCollisionListener
{

    [SerializeField] int damage = 10;

    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(other.tag != "Untagged" && other.tag != tag)
        {
            other.gameObject.GetComponent<IHealthStat>()?.TakeDamage(damage);
            other.gameObject.GetComponent<Animator>()?.SetTrigger("backoff");
        } 
    }
}
