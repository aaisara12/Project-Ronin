using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is the interface Ability talks to when ever they take effect.
/// Use the default if you want, but should inherit it to customize to need - this is just an interface for abilities to use.
/// </summary>
public class Attributes : MonoBehaviour
{
    /// <summary>
    /// Attributes will register themselves here.
    /// </summary>
    static public HashSet<Attributes> activeAttributes { get; protected set; } = new HashSet<Attributes>();
    
    /// <summary>
    /// Only notifies "change", no specific location of change provided.
    /// </summary>
    [SerializeField]
    private UnityEvent onAttributeChange;

    /// <summary>
    /// All attributes associated with attribute should be just float - add in child if necessary.
    /// </summary>
    protected Dictionary<string, float> floatAttributes = new Dictionary<string, float>();

    /// <summary>
    /// Tags enbales easier filtering.
    /// </summary>
    protected HashSet<string> attributeTags = new HashSet<string>();

    private void Awake()
    {
        activeAttributes.Add(this);
    }

    private void OnDestroy()
    {
        activeAttributes.Remove(this);
    }

    public float GetFloat(string name)
    {
        if (!floatAttributes.ContainsKey(name))
        {
            Debug.Log(gameObject);
            throw new System.Exception("Modifying unknown attribute \"" + name + "\"");
        }

        return floatAttributes[name];
    }

    public void ModifyFloat(string name, float delta)
    {
        if (!floatAttributes.ContainsKey(name))
        {
            Debug.Log(gameObject);
            throw new System.Exception("Modifying unknown attribute \"" + name + "\"");
        }

        floatAttributes[name] += delta;
        onAttributeChange.Invoke();
    }

    public void SetFloat(string name, float newValue)
    {
        if (!floatAttributes.ContainsKey(name))
        {
            Debug.Log("Warning: setting unknown attribute:");
            Debug.Log(gameObject);
            Debug.Log(name);
            Debug.Log("-----------------------------------");
        }

        floatAttributes[name] = newValue;
        onAttributeChange.Invoke();
    }

    public bool CheckTag(string tagName)
    {
        return attributeTags.Contains(tagName);
    }

    public void AddTag(string newTag, float duration = -1)
    {
        if (attributeTags.Contains(newTag))
        {
            Debug.Log("Warning: adding duplicate tag: " + newTag);
        }
        else
        {
            if (duration > 0)
            {
                StartCoroutine(RemoveTimer(newTag, duration));
            }

            attributeTags.Add(newTag);
        }
    }

    public void RemoveTag(string oldTag)
    {
        if (!attributeTags.Contains(oldTag))
        {
            Debug.Log("Warning: remove unknown tag: " + oldTag);
        }
        else
        {
            attributeTags.Remove(oldTag);
        }
    }

    private IEnumerator RemoveTimer(string oldTag, float deadline)
    {
        while (Time.time < deadline)
        {
            yield return null;
        }

        RemoveTag(oldTag);
    }
}
