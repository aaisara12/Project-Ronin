using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactionRange = 5.0f;

    InteractableObject lastClosestInteractable;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, interactionRange);

        Gizmos.color = Color.magenta;
        if (lastClosestInteractable != null)
        {
            Gizmos.DrawLine(gameObject.transform.position, lastClosestInteractable.gameObject.transform.position);
        }
    }


    /* TODO: Message flarros on discord:
     * I don't think this is going to link up correctly with how the InteractableObject class is being set up.
     * HMU when you get the chance! :)
    */
    void FixedUpdate()
    {
        InteractableObject closestInteractable = null;
        float minDistance = float.MaxValue;

        Collider[] potentialInteractables = Physics.OverlapSphere(gameObject.transform.position, interactionRange);

        foreach (Collider col in potentialInteractables)
        {
            // Jank version -- don't use!
            // Ideally, we want to have a dictionary of all interactable objects in the scene (key = GameObject, value = InteractableObject)
            // and try to find their InteractableObject component that way

            // Question: Is there any way around having to check for objects in the scene every physics frame? Is this an expensive
            // enough problem for us to have to worry about it?

            InteractableObject interactableObject = col.GetComponent<InteractableObject>();

            if (interactableObject != null)
            {
                //Debug.Log("Interactable object in range");
                float objectDist =
                    Vector3.Distance(gameObject.transform.position, interactableObject.transform.position);

                if (objectDist < minDistance)
                {
                    minDistance = objectDist;
                    closestInteractable = interactableObject;
                }
            }
        }

        if (closestInteractable != lastClosestInteractable)
        {
            closestInteractable?.SetInteractable(true); // Highlight closest interactable
            lastClosestInteractable?.SetInteractable(false);
        }

        lastClosestInteractable = closestInteractable;
    }

    /// <summary> Interact with the nearest InteractableObject in range </summary>
    public void InteractWithNearestInRange()
    {
        lastClosestInteractable?.TryInteract();
    }
}