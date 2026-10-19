using UnityEngine;

[CreateAssetMenu(fileName = "crossbow", menuName = "Scriptable Objects/crossbow_bolts")]
public class crossbow_bolts : ScriptableObject
{
    
    public int startValue = 6;
    public int value;

    void OnEnable() {
        value = startValue;
    }

    public void SpendBolt()
    {
        value -= 1;
    }

    public void ResetBolts()
    {
        value = startValue;
    }
}
