using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSummoner : MonoBehaviour
{
    [SerializeField] List<Spawner> spawners;
    [SerializeField] WaveManager waveManager;
    
    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }
    public void Summon()
    {
        foreach(Spawner s in spawners)
        {
            waveManager.AddUnitToWave(s.Spawn());
        }
    }
}
