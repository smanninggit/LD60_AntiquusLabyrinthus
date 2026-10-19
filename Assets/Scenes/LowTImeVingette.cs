using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LowTimeVignette : MonoBehaviour
{
    [SerializeField] timer torchTimer;         
    [SerializeField] Volume volume;          
    [SerializeField, Range(0f, 1f)] float lowThreshold = 0.15f;
    [SerializeField] float lowIntensity = 0.4f;

    Vignette vignette;
    float baseIntensity;

    void Start()
    {
        if (!volume.profile.TryGet(out vignette))
        {
            enabled = false;
            return;
        }
        baseIntensity = vignette.intensity.value;
    }

    void Update()
    {
        float start = (float)torchTimer.startValue;
        float fractionLeft = start > 0f ? (float)torchTimer.timeLeft / start : 0f;

        vignette.intensity.Override(fractionLeft < lowThreshold ? lowIntensity : baseIntensity);
    }
}