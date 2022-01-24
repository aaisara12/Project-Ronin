using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ability
{
    // inherited interface
    public abstract void Start();
    public abstract void Update();

    // target search functions
    protected void ForEachInRadius(float radius, Action<Attribute> action)
    {
        // TODO: search
        // TODO: for each result apply the action
    }

    // wild card function
    protected void ForEach(Func<IEnumerable<Attribute>> condition, Action<Attribute> action)
    {
        foreach (Attribute attr in condition())
        {
            action(attr);
        }
    }
}
