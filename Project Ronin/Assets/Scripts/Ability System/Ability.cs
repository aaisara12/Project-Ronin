using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Gameplay abilities, inherit this class to implement behavior.
/// Protected methods are provided to help search for attributes.
/// 
/// Instead of choosing one design approach, why not have both?
/// </summary>
public abstract class Ability : MonoBehaviour
{
    public GameObject prefab = null; // only set by ability pool
    public AttributeSet user { set; protected get; }

    /// <summary>
    /// Put any initialization here. Remember not to ignore the base implementation.
    /// By default, ability is synced with the user in position and rotation.
    /// </summary>
    /// <param name="inUser"></param>
    public virtual void InitiateAbility(AttributeSet inUser)
    {
        user = inUser;
        transform.position = user.transform.position;
        transform.rotation = user.transform.rotation;
    }

    /// <summary>
    /// Implement any reset logic here.
    /// </summary>
    public virtual void ResetAbility() { }

    /// <summary>
    /// Apply a function to all the Attributes objects.
    /// Use this function if you want some quick and broad effect (e.g. everyone within a certain range) cuz the monobehaviour approach might not be efficient enough.
    /// </summary>
    /// <param name="filter">if a given Attributes object should be operated on</param>
    /// <param name="action">action to apply to given Attribute</param>
    protected void ForEachAttr(Func<AttributeSet, bool> filter, Action<AttributeSet> action)
    {
        foreach (AttributeSet attr in AttributeSet.activeAttributes)
        {
            if (filter(attr))
            {
                action(attr);
            }
        }
    }

    /// <summary>
    /// Helper function, lookup a gameobject's attribute (cuz we don't want to grab it by GetComponent)
    /// </summary>
    /// <param name="owner">the object that owns the attribute you are looking for</param>
    /// <returns></returns>
    protected AttributeSet AttributeLookup(GameObject owner)
    {
        return AttributeSet.objectToAttributes[owner];
    }

    /// <summary>
    /// Call this function in Update() when the ability is done -> don't Destroy()!
    /// </summary>
    protected void RecycleAbility()
    {
        gameObject.SetActive(false);
        AbilityPool.PutAbility(this);
    }
}
