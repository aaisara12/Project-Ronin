using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string objectName = "";
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private float interactionRange = 3.0f;

    /// <summary>Event function to ensure the object has a name</summary>
    /// <remarks>Uses the gameObject name if objectName is empty</remarks>
    private void Awake()
    {
        if (objectName == "")
        {
            objectName = gameObject.name;
        }
    }


    /// <summary>Virtual function that defines behavior on interaction</summary>
    /// <remarks>Function must be overriden as no default behavior is defined</remarks>
    protected virtual void DoInteraction()
    {
        DisplayMessage();
        Debug.LogFormat("{0} has been interacted with!", gameObject.name);
    }


    /// <summary>Attempt to interact with this object</summary>
    /// <returns>true on success</returns>
    /// <remarks>Object might be an interactableObject but not yet interactable in scene</remarks>
    public bool TryInteract()
    {
        if (isInteractable)
        {
            DoInteraction();
            return true;
        }

        return false;
    }

    /// <summary>Display interaction text to screen</summary>
    /// <remarks>objectName must == filename</remarks>
    private void DisplayMessage()
    {
        // Stream in the associated file for input text if it exists
        string interactionText;
        try
        {
            interactionText = Application.streamingAssetsPath + "/InteractableObject/" + objectName + ".txt";
        }
        catch (Exception e)
        {
            interactionText = "";
            Debug.LogFormat("{0}.txt not found!", objectName);
        }

        // Display the text to the UI -- Using GameManager?
        GameManager gm = GameManager.Instance;
        if (gm != null && interactionText != "")
        {
            Debug.LogFormat("Interaction text: {0}", interactionText);
            //TODO: This function should either utilize the GameManager or a UIManager of some sort.
            // Questions:
            // 1) What object in scene will MainTextUI guarantee to be attached to? Seems like having a manager to
            //    always reference from would be easiest.
            // 2) Will we ever run into issues with queues of text? Perhaps an item is triggered or picked up while 
            //    another popup is already up? Or will these text boxes always temporarily pause the game?
        }
    }


    /// <summary>Make object interactable</summary>
    /// <param name = "state">Boolean </param> 
    /// <remarks>An objects use can be turned on or off via this function.</remarks>
    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }


    /// <summary>Event function to highlight an interactable object in scene</summary>
    private void OnDrawGizmos()
    {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(gameObject.transform.position, interactionRange);
    }
}