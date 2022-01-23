using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool isInteractable = true;

    // Use this function to define desired behavior on interaction
    protected abstract void DoInteraction();

    // Use this function to initiate interaction
    public void Interact()
    {
        if (isInteractable)
        {
            DoInteraction();
        }
    }    
    
    // Use to enable interaction 
    public void ActivateInteractable()
    {
        isInteractable = true;
    }
    
    // Use to disallow interaction
    public void DeactivateInteractable()
    {
        isInteractable = false;
    }
}
