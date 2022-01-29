using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionInteractableObject : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected override void DoInteraction()
    {
        // TODO
        Debug.LogFormat("I {0} have been interacted with!", gameObject.name);
    }
}
