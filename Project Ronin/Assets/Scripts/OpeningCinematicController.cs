using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCinematicController : RemoteCollisionListener
{
    [SerializeField] Animator cinematicAnimator;
    [SerializeField] CutsceneRelay relay;
    [SerializeField] PlayerBrain playerBrain;
    [SerializeField] WaveManager waveManager;
    bool hasTriggeredCinematic = false;

    void Start()
    {
        cinematicAnimator.SetTrigger("fade_in");
    }

    void OnEnable()
    {
        relay.OnCutsceneStart += HandleCutsceneStart;
        relay.OnCutsceneEnded += HandleCutsceneEnd;
    }

    void OnDestroy()
    {
        relay.OnCutsceneStart -= HandleCutsceneStart;
        relay.OnCutsceneEnded -= HandleCutsceneEnd;
    }

    protected override void OnTriggerEnterRemote(Collider other)
    {
        if(!hasTriggeredCinematic && other.CompareTag("Player"))
        {
            cinematicAnimator.SetTrigger("start");
            AudioManager.instance.SwapTrack("soundtrack-battle");
            waveManager.TryStartNextWave();
            hasTriggeredCinematic = true;
        }
        
    }

    void HandleCutsceneStart()
    {
        playerBrain.enabled = false;
    }

    void HandleCutsceneEnd()
    {
        playerBrain.enabled = true;
    }
}
