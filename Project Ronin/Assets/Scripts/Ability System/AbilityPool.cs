using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool for abilities.
/// </summary>
public class AbilityPool : MonoBehaviour
{
    // singleton interface
    public static AbilityPool instance { get; private set; } = null;
    
    public static Ability TakeAbility(GameObject prefab)
    {
        return instance.AllocateAbility(prefab);
    }

    public static void PutAbility(Ability abilityObj)
    {
        instance.FreeAbility(abilityObj);
    }

    // TODO: put stuff in there
    public GameObject dummyAttack;

    public GameObject attack;
    public GameObject parry;
    public GameObject dodge;

    // pooling
    private Dictionary<GameObject, Queue<Ability>> freeQueue = new Dictionary<GameObject, Queue<Ability>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private Ability AllocateAbility(GameObject prefab)
    {
        if (!freeQueue.ContainsKey(prefab))
        {
            freeQueue.Add(prefab, new Queue<Ability>());
        }

        if (freeQueue[prefab].Count <= 0)
        {
            freeQueue[prefab].Enqueue(Instantiate(prefab).GetComponent<Ability>());
            freeQueue[prefab].Peek().gameObject.SetActive(false);
            freeQueue[prefab].Peek().prefab = prefab;
        }

        freeQueue[prefab].Peek().gameObject.SetActive(true);
        return freeQueue[prefab].Dequeue();
    }

    private void FreeAbility(Ability abilityObj)
    {
        abilityObj.gameObject.SetActive(false);
        freeQueue[abilityObj.prefab].Enqueue(abilityObj);
    }
}
