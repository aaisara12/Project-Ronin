using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]

public class FadeCanvasGroup : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine = null;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private static IEnumerator Fade(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = startAlpha;

        float startTime = Time.unscaledTime;
        float progress = 0;

        while (true)
        {
            progress = (Time.unscaledTime - startTime) / duration;
            if (progress < 1)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            }
            else
            {
                canvasGroup.alpha = endAlpha;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void Stop()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = null;
    }

    public void FadeIn(float duration = 1f)
    {
        Stop();
        fadeCoroutine = StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 1f, duration));
    }

    public void FadeOut(float duration = 1f)
    {
        Stop();
        fadeCoroutine = StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 0f, duration));
    }
}
