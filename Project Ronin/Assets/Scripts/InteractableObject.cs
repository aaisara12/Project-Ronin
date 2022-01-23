using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool isInteractable = true;

    // Use this function to define desired behavior on interaction
    protected abstract void DoInteraction();

    // Use this function to initiate interaction
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
    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }
}
