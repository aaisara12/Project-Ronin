using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector
{
    Dictionary<string, float> floatModifications = new Dictionary<string, float>();
    Dictionary<string, float> floatOverrides = new Dictionary<string, float>();
    Dictionary<string, bool> tagChanges = new Dictionary<string, bool>();

    public void Apply(AttributeSet target)
    {
        foreach (var mod in floatModifications)
        {
            target.ModifyFloat(mod.Key, mod.Value);
        }

        foreach (var mod in floatOverrides)
        {
            target.ModifyFloat(mod.Key, mod.Value);
        }

        foreach (var mod in tagChanges)
        {
            if (mod.Value)
            {
                target.AddTag(mod.Key);
            }
            else
            {
                target.RemoveTag(mod.Key);
            }
        }
    }

    public void AddFloatOverride(string name, float overrider)
    {
        floatOverrides[name] = overrider;
    }

    public void AddTagChange(string tag, bool isAdding)
    {
        tagChanges[tag] = isAdding;
    }

    public void AddFloatModification(string name, float delta)
    {
        floatModifications[name] = delta;
    }
}
