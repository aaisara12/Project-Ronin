using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>Try to interact with this object</summary>
    public void TryInteract()
    {
        // TODO
        InteractAction();
    }

    /// <summary>Cause this interactable object to be highlighted if <paramref name = "isMarked"/> is true and not otherwise</summary>
    public void SetMark(bool isMarked)
    {
        // TODO
    }

    protected abstract void InteractAction();

    
}
