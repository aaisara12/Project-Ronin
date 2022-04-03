using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeMove : RemoteCollisionListener
{

    [SerializeField] float damage = 10;

    protected override void OnTriggerEnterRemote(Collider other)
    {
        var attSet = AttributeSet.objectToAttributes[other.gameObject];
        if(attSet != AttributeSet.objectToAttributes[gameObject])
        {
            //Debug.LogFormat("Hit {0}", other.name);
            attSet.ModifyFloat("hp", -damage);
            attSet.AddTag("backoff");
        }    

    }
}
