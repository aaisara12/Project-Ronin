using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is the interface Ability talks to when ever they take effect.
/// Use the default if you want, but should inherit it to customize to need - this is just an interface for abilities to use.
/// </summary>
public class AttributeSet : MonoBehaviour
{
    /// <summary>
    /// Attributes will register themselves here.
    /// </summary>
    static public HashSet<AttributeSet> activeAttributes { get; protected set; } = new HashSet<AttributeSet>();

    static public Dictionary<GameObject, AttributeSet> objectToAttributes { get; protected set; } = new Dictionary<GameObject, AttributeSet>();

    /// <summary>
    /// Listener to change. 
    /// Doesn't give specifically what changed. Ideally functions here listen to a specific thing so they don't need extra information.
    /// </summary>
    [SerializeField]
    private UnityEvent onAttributeChange;

    [SerializeField]
    private AttributeInitializer initializer;

    /// <summary>
    /// All attributes associated with attribute should be just float - add in child if necessary.
    /// </summary>
    protected Dictionary<string, float> floatAttributes = new Dictionary<string, float>();

    /// <summary>
    /// Tags enbales easier filtering.
    /// </summary>
    protected HashSet<string> attributeTags = new HashSet<string>();

    /// <summary>
    /// Used to manage auto-remove coroutines.
    /// </summary>
    private Dictionary<string, Coroutine> timerActivityMap = new Dictionary<string, Coroutine>();

    private void Awake()
    {
        activeAttributes.Add(this);
        objectToAttributes.Add(gameObject, this);

        if (initializer != null)
        {
            foreach (var ini in initializer.floatInitials)
            {
                floatAttributes.Add(ini.name, ini.value);
            }

            foreach (string tag in initializer.tagInitials)
            {
                attributeTags.Add(tag);
            }
        }
    }

    private void OnDestroy()
    {
        activeAttributes.Remove(this);
        objectToAttributes.Remove(gameObject);
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

    /// <summary>
    /// Add a new tag to attribute set. Pass in duration greater than 0 so 
    /// </summary>
    /// <param name="newTag"></param>
    /// <param name="duration"></param>
    public void AddTag(string newTag, float duration = -1)
    {
        if (attributeTags.Contains(newTag))
        {
            Debug.Log("Warning: adding duplicate tag: " + newTag);

            if (timerActivityMap.ContainsKey(newTag))
            {
                StopCoroutine(timerActivityMap[newTag]);
                timerActivityMap.Remove(newTag);
            }
        }

        if (duration > 0)
        {
            timerActivityMap.Add(newTag, StartCoroutine(RemoveTimer(newTag, duration)));
        }

        attributeTags.Add(newTag);
        
        onAttributeChange.Invoke();
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
        onAttributeChange.Invoke();
    }

    private IEnumerator RemoveTimer(string oldTag, float duration)
    {
        float deadline = Time.time + duration;

        while (Time.time < deadline)
        {
            yield return null;
        }

        RemoveTag(oldTag);
    }
}
