using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;

    public HealthStat Spawn()
    {
        return Instantiate<GameObject>(creaturePrefab, transform.position, transform.rotation).GetComponent<HealthStat>();
    }
}
