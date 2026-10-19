using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        Setup(masterSlider, "MasterVol");
        Setup(musicSlider, "MusicVol");
        Setup(sfxSlider, "SFXVol");
    }

    void Setup(Slider slider, string param)
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;

        if (mixer.GetFloat(param, out float db))
            slider.SetValueWithoutNotify(DbToLinear(db));

        slider.onValueChanged.AddListener(v => SetVolume(param, v));
    }

    void SetVolume(string param, float value)
    {
        float db = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat(param, db);
    }

    static float DbToLinear(float db)
    {
        return db <= -80f ? 0f : Mathf.Pow(10f, db / 20f);
    }
}