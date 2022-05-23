using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStomp : RemoteCollisionListener
{
    [SerializeField] int damage = 30;

    void Start()
    {
        GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
    }

    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(other.tag != "Untagged" && other.tag != tag)
        {
            other.gameObject.GetComponent<IHealthStat>()?.TakeDamage(damage);
            other.gameObject.GetComponent<Animator>().SetTrigger("knocked");
        }
         
    }
}
