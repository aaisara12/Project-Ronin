using UnityEngine;
using TMPro;

public class MainTextUI : PausableUI
{
    [Header("GameObjects Dependencies")] [SerializeField]
    private TextMeshProUGUI mainText;

    [SerializeField] private CanvasGroup background;
    [SerializeField] private RectTransform box;

    [Header("Main Text Settings")] public float yPos = -200f;
    public float fadeTime = 0.5f;

    private const int FULL_ALPHA = 1;
    private const int TRANSPARENT_ALPHA = 0;

    /// <summary> Displays <paramref name = "text"/> on the screen's main UI field </summary>
    public void DisplayText(string text)
    {
        // Set display text
        mainText.text = text;

        // Fade in background
        background.alpha = TRANSPARENT_ALPHA;
        background.LeanAlpha(FULL_ALPHA, fadeTime);

        // Animate box to the screen
        box.localPosition = new Vector2(0, -Screen.height - box.rect.height);
        box.LeanMoveLocalY(yPos, fadeTime).setEaseOutExpo().delay = 0.1f;

        RequestPause();
    }

    /// <summary> Proceed to next step in this dialogue's box state machine </summary>
    public void Continue()
    {
        CloseText();
    }

    /// <summary> Clears main text <paramref name = "text"/> on the screen's main UI field </summary>
    private void CloseText()
    {
        box.LeanMoveLocalY(-Screen.height - box.rect.height, fadeTime).setEaseInExpo();
        background.LeanAlpha(TRANSPARENT_ALPHA, fadeTime).delay = 0.1f;

        RequestUnpause();
    }
}