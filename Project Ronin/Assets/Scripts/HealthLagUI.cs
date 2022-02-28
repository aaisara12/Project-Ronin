using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLagUI : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float catchUpSpeed = 0.5f;
    [SerializeField] UnityEngine.UI.Slider referenceHealthSlider;
    UnityEngine.UI.Slider slider;

    float targetValue = 1;

    void Awake()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();
        referenceHealthSlider.onValueChanged.AddListener(SetTargetPercentage);
    }

    public void SetTargetPercentage(float newTarget)
    {
        targetValue = newTarget;
    }

    void Update()
    {
        float diff = targetValue - slider.value;

        bool isLaggedBarShowing = diff > 0 && diff < 0.1f;

        if(!isLaggedBarShowing)
        {
            float step = Mathf.Sign(diff) * catchUpSpeed * Time.deltaTime;

            slider.value += step;
        }
    }
}
