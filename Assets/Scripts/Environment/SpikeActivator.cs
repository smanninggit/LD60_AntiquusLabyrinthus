using UnityEngine;
using UnityEngine.Events;

public class SpikeActivator : MonoBehaviour
{
    public UnityEvent onInteract;
    public UnityEvent onExit;


    public void Interact() { 
            onInteract.Invoke();
    }

    void OnTriggerEnter(){
        Interact();
    }

    void OnTriggerExit(){
        onExit.Invoke();
    }
}
