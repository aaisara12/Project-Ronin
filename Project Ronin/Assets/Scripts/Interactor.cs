using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactionRange = 5.0f;

    InteractableObject lastClosestInteractable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            InteractWithNearestInRange();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, interactionRange);
        Gizmos.color = Color.magenta;
    }

    void FixedUpdate()
    {
        
        InteractableObject closestInteractable = null;
        float minDistance = float.MaxValue;

        Collider[] potentialInteractables = Physics.OverlapSphere(gameObject.transform.position, interactionRange);

        foreach(Collider col in potentialInteractables)
        {
            // Jank version -- don't use!
            InteractableObject interactableObject = col.GetComponent<InteractableObject>();

            if(interactableObject != null)
            {
                //Debug.Log("Interactable object in range");
                float objectDist = Vector3.Distance(gameObject.transform.position, interactableObject.transform.position);

                if(objectDist < minDistance)
                {
                    minDistance = objectDist;
                    closestInteractable = interactableObject;
                }
            }
        }

        if(closestInteractable != lastClosestInteractable)
        {
            closestInteractable?.SetInteractable(true);      // Highlight closest interactable
            lastClosestInteractable?.SetInteractable(false);
        }

        lastClosestInteractable = closestInteractable;


    }

    public void InteractWithNearestInRange()
    {
        lastClosestInteractable?.TryInteract();
    }

}
