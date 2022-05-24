using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] int currentWave = 0;
    [SerializeField] bool autoStart = true;

    public event System.Action<int> OnStartWave;
    public event System.Action<int> OnClearedWave;

    List<HealthStat> unitsSpawned = new List<HealthStat>();

    void Start()
    {
        if(autoStart)
            TrySpawnNextWave();
    }


    public bool TrySpawnNextWave()
    {
        if(currentWave < waves.Count)
        {
            AudioManager.instance.PlaySound("wave-spawn");
            currentWave++;
            OnStartWave?.Invoke(currentWave);

            // Spawn enemy from each spawner location
            foreach(Spawner s in waves[currentWave - 1].spawners)
            {
                HealthStat healthStat = s.Spawn();
                unitsSpawned.Add(healthStat);
                healthStat.OnDied += HandleUnitDied;
            }

            if(waves[currentWave - 1].spawners.Count == 0)
            {
                Debug.LogWarning("No spawners specified for wave " + currentWave + "!");
                TrySpawnNextWave();
            }

            return true;
        }
        return false;
    }

    void HandleUnitDied(HealthStat healthStat)
    {
        unitsSpawned.Remove(healthStat);
        healthStat.OnDied -= HandleUnitDied;

        if(unitsSpawned.Count == 0)
        {
            OnClearedWave?.Invoke(currentWave);
            TrySpawnNextWave();
        }
    }


}

[System.Serializable]
public struct Wave
{
    public List<Spawner> spawners;
}
