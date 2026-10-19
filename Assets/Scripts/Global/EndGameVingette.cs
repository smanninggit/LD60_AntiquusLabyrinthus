using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EndGameVingette : MonoBehaviour
{
    public Volume volume;
    public float duration = 5f;
    public timer torchTimer;
    public Image blackout;                    
    [Range(0f, 1f)] public float blackoutStart = 0.7f;
    public CanvasGroup uiGroup;       
    [Range(0f, 1f)] public float uiFadeStart = 0f;  

    Vignette vignette;
    float t;
    bool running;
    bool started;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
        vignette.intensity.value = 0f;

        SetBlackoutAlpha(0f);
    }

    public void Begin() { t = 0f; running = true; started = true; }

    void Update()
    {
        if (!started && torchTimer.timeLeft <= 0f) Begin();
        if (!running) return;

        t += Time.deltaTime / duration;
        float p = Mathf.Clamp01(t);

        vignette.intensity.value = Mathf.Lerp(0f, 1f, p);
        vignette.smoothness.value = Mathf.Lerp(0.2f, 1f, p);

        uiGroup.alpha = 1f - Mathf.InverseLerp(uiFadeStart, 1f, p);

        float a = Mathf.InverseLerp(blackoutStart, 1f, p);
        SetBlackoutAlpha(a);

        if (t >= 1f) running = false;
    }

    void SetBlackoutAlpha(float a)
    {
        Color c = blackout.color;
        c.a = a;
        blackout.color = c;
    }
}