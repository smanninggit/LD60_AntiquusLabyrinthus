using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public health playerHealth;
    public Image fillImage; 

    void Update()
    {
        fillImage.fillAmount = (float)playerHealth.value / playerHealth.startValue;
    }
}