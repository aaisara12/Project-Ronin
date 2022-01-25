using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Gameplay abilities, inherit this class to implement behavior.
/// Protected methods are provided to help search for attributes.
/// 
/// TODO: discuss the possibility of abilities dependend on other abilities.
/// </summary>
public abstract class Ability : MonoBehaviour
{
    public abstract void ResetAbility();

    /// <summary>
    /// Apply a function to all the Attributes objects.
    /// </summary>
    /// <param name="filter">if a given Attributes object should be operated on</param>
    /// <param name="action">action to apply to given Attribute</param>
    protected void ForEachAttr(Func<Attributes, bool> filter, Action<Attributes> action)
    {
        foreach (Attributes attr in Attributes.activeAttributes)
        {
            if (filter(attr))
            {
                action(attr);
            }
        }
    }
}
