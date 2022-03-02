using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Designed to hide behind effectors, didn't really protect access to this class but do try to avoid accessing them directly.
/// Keep in mind changes made here are scene-wide global.
/// </summary>
public abstract class CombatMechanismBase
{
    static private List<CombatMechanismBase> mechanismQueueBackup = null;
    static public List<CombatMechanismBase> mechanismQueue { get; private set; } = new List<CombatMechanismBase>();

    static CombatMechanismBase()
    {
        // put default mechanism list here
    }

    static public void SwapQueue(List<CombatMechanismBase> newQueue)
    {
        if (mechanismQueueBackup == null) 
            mechanismQueueBackup = mechanismQueue;

        mechanismQueue = newQueue;
    }

    static public void RestoreQueue()
    {
        if (mechanismQueueBackup == null)
        {
            throw new Exception("Restoring before backing up!");
        }

        mechanismQueue = mechanismQueueBackup;
    }

    public abstract void Process(ref MechanismIterateContext context, Effector effector);
    public abstract bool Filter(Effector effector);
}
