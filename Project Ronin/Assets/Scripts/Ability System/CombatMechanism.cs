using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit this class to create custom combat mechanisms.
/// </summary>
/// <typeparam name="ChildType">the child type inheriting this class</typeparam>
public abstract class CombatMechanism<ChildType> : CombatMechanismBase
{
    public virtual string GetSigniture()
    {
        return typeof(ChildType).Name;
    }

    public override bool Filter(Effector effector)
    {
        return effector.signatures.Contains(GetSigniture());
    }
}
