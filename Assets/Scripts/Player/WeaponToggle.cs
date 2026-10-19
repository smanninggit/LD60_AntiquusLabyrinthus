using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponToggle : MonoBehaviour
{
    public GameObject[] firstSet;  
    public GameObject[] secondSet; 

    bool firstActive = true; 

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame){
            firstActive = true;
            SetActive(secondSet, false, firstSet, true);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame){
            firstActive = false;
            SetActive(secondSet, true, firstSet, false);
        }


        if (Mouse.current != null && Mouse.current.scroll.ReadValue().y != 0f){
            if (firstActive){
                firstActive = false;
                SetActive(secondSet, true, firstSet, false);
            }
            else {
                firstActive = true;
            SetActive(secondSet, false, firstSet, true);
            }
        }
    }

    void SetActive(GameObject[] a, bool aState, GameObject[] b, bool bState)
    {
        foreach (var go in a) if (go != null) go.SetActive(aState);
        foreach (var go in b) if (go != null) go.SetActive(bState);
    }
}