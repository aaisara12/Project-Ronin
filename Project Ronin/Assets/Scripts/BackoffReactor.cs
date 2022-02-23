using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackoffReactor : MonoBehaviour
{
    AttributeSet attributeSet;
    UnityAction handleBackoffTag;
    Animator animator;

    bool didTagExist = false;      // Is the tag still on the attribute? (need this until we implement previous/current value)

    void Start()
    {
        animator = GetComponent<Animator>();

        handleBackoffTag = HandleBackoffTag;

        if(AttributeSet.objectToAttributes.ContainsKey(gameObject))
        {
            attributeSet = AttributeSet.objectToAttributes[gameObject];
            attributeSet.RegisterOnAttributeChange(handleBackoffTag);
        }
    }

    void OnDisable()
    {
        attributeSet?.DeregisterOnAttributeChange(handleBackoffTag);
    }

    void HandleBackoffTag()
    {
        if(!didTagExist && attributeSet.CheckTag("backoff"))
        {
            animator?.SetTrigger("backoff");
            // Play any "backoff" sounds?

            didTagExist = true;
        }

        // Tag has disappeared (once again, we shouldn't need this if we get info about previous/current state)
        if(didTagExist && !attributeSet.CheckTag("backoff"))
        {
            didTagExist = false;
        }
        
    }
}
