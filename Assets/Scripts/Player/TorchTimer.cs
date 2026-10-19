using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TorchTimer : MonoBehaviour
{
    public timer torchTimer;  
    public Image fillImage; 
    public float fadeDuration = 1f;
    public Image fadeImage;

    bool ending;

    void Update()
    {
        if (torchTimer.timeLeft <= 0f) return;
        torchTimer.timeLeft -= Time.deltaTime;

        fillImage.fillAmount = (float)torchTimer.timeLeft / torchTimer.startValue;

        if (torchTimer.timeLeft <= 0f) { torchTimer.timeLeft = 0f; OnTimerEnd(); }
    }

    private void OnTimerEnd(){
        if (ending) return;
        ending = true;
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        Color c = fadeImage.color;
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0.4f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene("EndScene");
    }
}