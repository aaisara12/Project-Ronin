using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;

    public HealthStat Spawn()
    {
        var creature = Instantiate<GameObject>(creaturePrefab, transform.position, transform.rotation).GetComponent<HealthStat>();
        creature.GetComponent<Demo.Enemy.PatrolPoints>().Initialize(new List<Transform> {transform});
        return creature;
    }
}
