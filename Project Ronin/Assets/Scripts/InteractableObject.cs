using System;
using UnityEngine;

/// <summary>
/// Attach to interactable objects. Handles glow upon object selection as well as sending a message to the UI
/// upon interaction. Inherit from this class and override DoInteraction() for more complex behavior.
/// </summary>
/// <remarks>
/// The objectName variable must match the file in StreamingAssets to display correct text to the UI. 
/// </remarks>
public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string objectName = "";
    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private float interactionRange = 3.0f;

    private MainTextUI textUI;

    /// <summary>Event function to ensure the object has a name</summary>
    /// <remarks>Uses the gameObject name if objectName is empty</remarks>
    private void Awake()
    {
        textUI = FindObjectOfType<MainTextUI>();
        if (objectName == "")
        {
            objectName = gameObject.name;
        }
    }


    /// <summary>Virtual function that defines behavior on interaction</summary>
    /// <remarks>Without overriding, will only display a message to the UI</remarks>
    protected virtual void DoInteraction()
    {
        DisplayMessage();
        Debug.LogFormat("{0} has been interacted with!", objectName);
    }


    /// <summary>Attempt to interact with this object</summary>
    /// <returns>true on success</returns>
    /// <remarks>Object might be an interactableObject but not yet interactable in scene</remarks>
    public bool TryInteract()
    {
        if (isInteractable && isSelected)
        {
            DoInteraction();
            return true;
        }

        return false;
    }

    /// <summary>Display interaction text to screen</summary>
    /// <remarks>objectName must == filename</remarks>
    protected void DisplayMessage()
    {
        // Stream in the associated file for display text if it exists
        string interactionText;
        try
        {
            interactionText = Application.streamingAssetsPath + "/InteractableObjects/" + objectName + ".txt";
        }

        // Unable to find a text file matching the objectName variable
        catch (Exception e)
        {
            interactionText = "";
            Debug.LogException(e, this);
        }

        // Display the text to the UI
        if (textUI != null && interactionText != "")
        {
            textUI.DisplayText(interactionText);
            Debug.LogFormat("Interaction text: {0}", interactionText);
        }
    }


    /// <summary>Make object interactable</summary>
    /// <param name = "state">Boolean</param> 
    /// <remarks>An object's use can be turned on or off via this function.</remarks>
    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }


    /// <summary>Update object selection</summary>
    /// <param name = "state">Boolean</param> 
    /// <remarks>Used to show if an object is currently being targeted</remarks>
    public void SetSelected(bool state)
    {
        isSelected = state;
    }


    /// <summary>Event function to highlight an interactable object in scene</summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gameObject.transform.position, interactionRange);
    }
}