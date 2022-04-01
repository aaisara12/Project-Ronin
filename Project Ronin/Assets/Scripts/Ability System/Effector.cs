using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A POD class that carries pending changes from user to target.
/// 
/// Let's abuse reflection one more time!
/// </summary>
public class Effector
{
    public AttributeSet user = null;
    public List<AttributeSet> targets = null;

    public Dictionary<string, float> floatModifications = new Dictionary<string, float>();
    public Dictionary<string, float> floatOverrides = new Dictionary<string, float>();
    public Dictionary<string, bool> tagChanges = new Dictionary<string, bool>();

    /// <summary>
    /// signature of systems (usually their type names, mechanisms can override this default behavior).
    /// </summary>
    public HashSet<string> signatures = new HashSet<string>();

    /// <summary>
    /// Submit this effector to mechenism queue.
    /// </summary>
    public void Submit()
    {
        MechanismIterateContext context = new MechanismIterateContext();

        foreach (var mechanism in CombatMechanismBase.mechanismQueue)
        {
            if (!context.shouldContinue)
            {
                break;
            }

            if (mechanism.Filter(this))
            {
                mechanism.Process(ref context, this);
            }
        }

        if (context.shouldApply)
        {
            Apply();
        }
    }

    /// <summary>
    /// In case you want to extend the system without changing the core, this method allows u to apply special logic by override.
    /// </summary>
    protected void Apply()
    {
        foreach (AttributeSet target in targets)
        {
            foreach (var mod in floatModifications)
            {
                target.QuietModifyFloat(mod.Key, mod.Value);
            }

            foreach (var mod in floatOverrides)
            {
                target.QuietSetFloat(mod.Key, mod.Value);
            }

            foreach (var mod in tagChanges)
            {
                if (mod.Value)
                {
                    target.QuietAddTag(mod.Key);
                }
                else
                {
                    target.QuietRemoveTag(mod.Key);
                }
            }

            target.SignalChange();
        }
    }

    /// <summary>
    /// Get string name of a mechanism class.
    /// </summary>
    /// <typeparam name="MechanismType">the mechanism class</typeparam>
    /// <returns>string name of the given nechanism class</returns>
    public string SerializeMechanismType<MechanismType>() where MechanismType : CombatMechanism<MechanismType>
    {
        return typeof(MechanismType).Name;
    }

    /// <summary>
    /// Add a signature.
    /// </summary>
    /// <typeparam name="MechanismType">the mechanism class</typeparam>
    /// <returns>string name of the given nechanism class</returns>
    public string Addsignature<MechanismType>() where MechanismType : CombatMechanism<MechanismType>
    {
        string newsignature = SerializeMechanismType<MechanismType>();

        signatures.Add(newsignature);

        return newsignature;
    }

    /// <summary>
    /// Remove a signature.
    /// </summary>
    /// <typeparam name="MechanismType"></typeparam>
    /// <returns></returns>
    public string Removesignature<MechanismType>() where MechanismType : CombatMechanism<MechanismType>
    {
        string newsignature = SerializeMechanismType<MechanismType>();

        signatures.Remove(newsignature);

        return newsignature;
    }
}
