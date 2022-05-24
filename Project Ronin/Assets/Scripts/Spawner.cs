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
        //StartCoroutine(FreezeMovementForTime(creature.GetComponent<Animator>(), 3));
        return creature;
    }

    // IEnumerator FreezeMovementForTime(Animator animator, float sec)
    // {
    //     animator.SetFloat("xInput", 0);
    //     yield return new WaitForSeconds(sec);
    //     animator.SetFloat("xInput", 1);
    // }
}
