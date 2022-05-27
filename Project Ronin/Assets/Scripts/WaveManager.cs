using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] int currentWave = 0;
    [SerializeField] bool autoStart = true;
    [SerializeField] float timeBeforeSpawn = 3f;

    public event System.Action<int> OnStartWave;
    public event System.Action OnStartFinalWave;    // So game knows when to play boss music
    public event System.Action<int> OnClearedWave;
    public event System.Action OnClearedFinalWave;

    List<HealthStat> unitsSpawned = new List<HealthStat>();

    void Start()
    {
        if(autoStart)
            TryStartNextWave();
    }


    public bool TryStartNextWave()
    {
        if(currentWave < waves.Count)
        {
            currentWave++;
            OnStartWave?.Invoke(currentWave);
            if(currentWave == waves.Count)
            {
                OnStartFinalWave?.Invoke();
            }
                
            StartCoroutine(DelayedSpawn());

            return true;
        }
        return false;
    }

    // For spawning units after initial wave start
    public void AddUnitToWave(HealthStat unitHealthStat)
    {
        unitsSpawned.Add(unitHealthStat);
        unitHealthStat.OnDied += HandleUnitDied;
    }

    void SpawnUnits()
    {
        AudioManager.instance.PlaySound("wave-spawn");
        // Spawn enemy from each spawner location
        foreach (Spawner s in waves[currentWave - 1].spawners)
        {
            HealthStat healthStat = s.Spawn();
            unitsSpawned.Add(healthStat);
            healthStat.OnDied += HandleUnitDied;
        }

        if (waves[currentWave - 1].spawners.Count == 0)
        {
            Debug.LogWarning("No spawners specified for wave " + currentWave + "!");
            TryStartNextWave();
        }
    }

    void HandleUnitDied(HealthStat healthStat)
    {
        unitsSpawned.Remove(healthStat);
        healthStat.OnDied -= HandleUnitDied;

        if(unitsSpawned.Count == 0)
        {
            StartCoroutine(SlowKill());
            OnClearedWave?.Invoke(currentWave);
            if(currentWave == waves.Count)
                OnClearedFinalWave?.Invoke();
            TryStartNextWave();
        }
    }

    IEnumerator SlowKill()
    {
        AudioManager.instance.PlaySound("hit-final");
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }
    
    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(timeBeforeSpawn);
        SpawnUnits();
    }


}

[System.Serializable]
public struct Wave
{
    public List<Spawner> spawners;
}
