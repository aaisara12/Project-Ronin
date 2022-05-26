using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCinematicController : RemoteCollisionListener
{
    [SerializeField] Animator cinematicAnimator;
    bool hasTriggeredCinematic = false;
    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(!hasTriggeredCinematic && other.CompareTag("Player"))
        {
            AudioManager.instance.SwapTrack("soundtrack-battle");
            hasTriggeredCinematic = true;
        }
        
    }
}
