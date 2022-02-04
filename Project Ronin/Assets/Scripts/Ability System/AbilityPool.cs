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

    [System.Obsolete("Use methods that take in user attribute.")]
    public static Ability TakeAbility(GameObject prefab)
    {
        return instance.AllocateAbility(prefab);
    }

    public static Ability TakeAbility(GameObject prefab, AttributeSet inUser)
    {
        var newAbility = instance.AllocateAbility(prefab);
        newAbility.InitiateAbility(inUser);
        return newAbility;
    }

    public static Ability TakeAbility(GameObject prefab, GameObject inUser)
    {
        var newAbility = instance.AllocateAbility(prefab);
        newAbility.InitiateAbility(AttributeSet.objectToAttributes[inUser]);
        return newAbility;
    }

    public static void PutAbility(Ability abilityObj)
    {
        instance.FreeAbility(abilityObj);
    }

    // TODO: put stuff in there
    public GameObject dummyAttack;
    public GameObject dummyShoot;

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

    private void ResetAbilityObject(Ability ability)
    {
        ability.gameObject.SetActive(false);
        ability.transform.SetParent(transform);
    }

    private void ResetAbilityObject(Ability ability, GameObject prefab)
    {
        ability.gameObject.SetActive(false);
        ability.prefab = prefab;
        ability.transform.SetParent(transform);
    }

    private Ability AllocateAbility(GameObject prefab)
    {
        if (!freeQueue.ContainsKey(prefab))
        {
            freeQueue.Add(prefab, new Queue<Ability>());
        }

        if (freeQueue[prefab].Count <= 0)
        {
            var newBility = Instantiate(prefab).GetComponent<Ability>();
            ResetAbilityObject(newBility, prefab);
            freeQueue[prefab].Enqueue(newBility);
        }

        freeQueue[prefab].Peek().gameObject.SetActive(true);
        freeQueue[prefab].Peek().transform.SetParent(null);
        return freeQueue[prefab].Dequeue();
    }

    private void FreeAbility(Ability returner)
    {
        ResetAbilityObject(returner);
        returner.ResetAbility();
        freeQueue[returner.prefab].Enqueue(returner);
    }
}
