using UnityEngine;

[CreateAssetMenu(fileName = "health", menuName = "Scriptable Objects/health")]
public class health : ScriptableObject
{
    
    public int startValue = 100;
    public int value;

    void OnEnable() {
        value = startValue;
    }

    public void TakeDamage(int amount)
    {
        value = Mathf.Max(0, value - amount);
    }

    public void ResetHealth()
    {
        value = startValue;
    }
}
