using TMPro;
using UnityEngine;

public class EndTimeDisplay : MonoBehaviour
{
    [SerializeField] timer torchTimer;
    [SerializeField] TMP_Text label;
    [SerializeField, Range(0f, 1f)] float goldThreshold = 0.5f;    
    [SerializeField, Range(0f, 1f)] float silverThreshold = 0.25f;  

    void Start()
    {
        float start = (float)torchTimer.startValue;
        float left = (float)torchTimer.timeLeft;

        float elapsed = Mathf.Max(0f, start - left);
        float fractionLeft = start > 0f ? left / start : 0f;

        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);

        string medal = fractionLeft > goldThreshold ? "Gold"
                     : fractionLeft > silverThreshold ? "Silver"
                     : "Bronze";

        label.text = $"You took {minutes}:{seconds:00} to do the dungeon\nYou earned a {medal} time!";

        torchTimer.timeLeft = torchTimer.startValue;
    }
}