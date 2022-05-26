using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCinematicController : MonoBehaviour
{
    [SerializeField] Animator cinematicAnimator;
    [SerializeField] HealthStat roninHealth;

    void Awake()
    {
        roninHealth.OnDied += HandleDeath;
    }

    private void HandleDeath(HealthStat healthStat)
    {
        StartCoroutine(LoadNextScene());

    }

    // This function is reused, and should be shared in a single location -- will fix if time
    IEnumerator LoadNextScene()
    {
        AudioManager.instance.SwapTrack("empty");
        yield return new WaitForSeconds(3);
        cinematicAnimator.SetTrigger("fade_out");
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

    }
}
