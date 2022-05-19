using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeMove : RemoteCollisionListener
{

    [SerializeField] float damage = 10;

    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(!AttributeSet.objectToAttributes.ContainsKey(other.gameObject)) {return;}

        var attSet = AttributeSet.objectToAttributes[other.gameObject];
        if(attSet != AttributeSet.objectToAttributes[gameObject])
        {
            attSet.ModifyFloat("hp", -damage);
            other.gameObject.GetComponent<Animator>().SetTrigger("backoff");
        }    

    }
}
