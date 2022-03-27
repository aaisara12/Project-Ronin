using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMelee : RemoteCollisionListener
{
    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(other.tag == "Enemy")
            Debug.Log("Hit enemy!");
    }
}
