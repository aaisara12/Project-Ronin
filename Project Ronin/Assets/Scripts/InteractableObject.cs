using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool isInteractable = true;

    // Use this function to define desired behavior on interaction
    protected abstract void DoInteraction();

    // Use this function to initiate interaction
    /// <summary>Try to interact with this object, return whether interaction was successful</summary>
    public bool TryInteract()
    {
        if (isInteractable)
        {
            DoInteraction();
            return true;
        }

        return false;
    }

    // Use to enable or disable interaction
    /// <summary>Make object interactable if <paramref name = "state"/> is true, and false otherwise</summary>
    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }
}
