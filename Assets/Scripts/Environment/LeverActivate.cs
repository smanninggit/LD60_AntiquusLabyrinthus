using UnityEngine;
using UnityEngine.Events;

public class LeverActivate : MonoBehaviour
{

    public UnityEvent onInteract;
    public Animator leverAnimator;
    bool doOnce = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;


    public void Interact() { 
        Debug.Log(doOnce);
        if (doOnce == false){
            Debug.Log(doOnce);
            Debug.Log("Cool");
            onInteract.Invoke();
            Debug.Log("Should set");
            audioSource.PlayOneShot(audioClip); 
            leverAnimator.SetTrigger("activate"); }
            doOnce = true;
        }
        
}