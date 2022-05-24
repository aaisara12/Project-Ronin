using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class SettingsOpener : MonoBehaviour
{

    [SerializeField] private FadeCanvasGroup fader;
    private PlayerInput playerInput;

    void Awake()
    {
        if (fader == null)
        {
            Debug.LogWarning("SettingsOpener on Player missing reference to Settings canvas.");
            Destroy(this);
        }
        playerInput = GetComponent<PlayerInput>();
    }

    public void Options(InputAction.CallbackContext context)
    {
        fader.FadeIn(1f);
        CanvasGroup cg = fader.gameObject.GetComponent<CanvasGroup>();
        cg.interactable = true;
        cg.blocksRaycasts = true;
        playerInput.enabled = false;
        Settings.paused = true;
        Time.timeScale = 0f;
    }
}
