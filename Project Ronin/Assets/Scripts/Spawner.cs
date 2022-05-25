using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] float spawnDelay = 1;

    public HealthStat Spawn()
    {
        Instantiate<GameObject>(spawnPrefab, transform.position, transform.rotation);
        var creature = Instantiate<GameObject>(creaturePrefab, transform.position, transform.rotation).GetComponent<HealthStat>();
        creature.gameObject.SetActive(false);
        StartCoroutine(DelaySpawn(creature.gameObject));
        creature.GetComponent<Demo.Enemy.PatrolPoints>().Initialize(new List<Transform> {transform});
        return creature;
    }

    IEnumerator DelaySpawn(GameObject creature)
    {
        yield return new WaitForSeconds(spawnDelay);
        creature.SetActive(true);

    }

}
