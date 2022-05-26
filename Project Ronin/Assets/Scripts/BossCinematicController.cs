using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCinematicController : MonoBehaviour
{
    [SerializeField] Animator cinematicAnimator;
    [SerializeField] WaveManager waveManager;
    [SerializeField] CutsceneRelay relay;
    [SerializeField] PlayerBrain playerBrain;

    void OnEnable()
    {
        relay.OnCutsceneStart += HandleCutsceneStart;
        relay.OnCutsceneEnded += HandleCutsceneEnd;
        waveManager.OnStartFinalWave += HandleFinalWave;
        waveManager.OnClearedFinalWave += HandleFinalWaveEnd;
    }

    private void HandleFinalWaveEnd()
    {
        GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
        AudioManager.instance.PlaySound("final-kill");
        cinematicAnimator.SetTrigger("final_kill");
        playerBrain.enabled = false;
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);
        cinematicAnimator.SetTrigger("fade_out");
    }

    void OnDestroy()
    {
        relay.OnCutsceneStart -= HandleCutsceneStart;
        relay.OnCutsceneEnded -= HandleCutsceneEnd;
        waveManager.OnStartFinalWave -= HandleFinalWave;
        waveManager.OnClearedFinalWave -= HandleFinalWaveEnd;
    }

    private void HandleFinalWave()
    {
        cinematicAnimator.SetTrigger("start");
        AudioManager.instance.SwapTrack("soundtrack-boss");
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
