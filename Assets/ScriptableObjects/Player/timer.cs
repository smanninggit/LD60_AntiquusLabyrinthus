using UnityEngine;

[CreateAssetMenu(fileName = "timer", menuName = "Scriptable Objects/timer")]
public class timer : ScriptableObject
{
    
    public float startValue = 100;
    public float timeLeft;

    void OnEnable() {
        timeLeft = startValue;
    }



    public void ResetTimer()
    {
        timeLeft = startValue;
    }
}
