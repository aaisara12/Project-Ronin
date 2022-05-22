using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeMove : RemoteCollisionListener
{

    [SerializeField] int damage = 10;
    [SerializeField] int knockback = 0;


    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(other.tag != "Untagged" && other.tag != tag)
        {
            other.gameObject.GetComponent<IHealthStat>()?.TakeDamage(damage);
            other.gameObject.GetComponent<AnimatorTriggerProxy>()?.RequestTrigger("backoff");

            // Use animator as a channel for communication with knockback code
            other.gameObject.GetComponent<Animator>()?.SetFloat("xKnock", transform.forward.x);
            other.gameObject.GetComponent<Animator>()?.SetFloat("yKnock", transform.forward.z);
            other.gameObject.GetComponent<Animator>()?.SetFloat("powKnock", knockback);
        } 
    }
}
