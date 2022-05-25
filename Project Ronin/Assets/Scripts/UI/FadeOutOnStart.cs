using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(FadeCanvasGroup))]

public class FadeOutOnStart : MonoBehaviour
{

    private FadeCanvasGroup fader;

    void Awake() => fader = GetComponent<FadeCanvasGroup>();
    void Start() => fader.FadeOut();
}
