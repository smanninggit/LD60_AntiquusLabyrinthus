using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private float moveThreshold = 0.5f; 
    [SerializeField] private float walkPitch = 1f;
    [SerializeField] private float sprintPitch = 5.4f;
    [SerializeField] private float sprintSpeedThreshold = 4.0f; 

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        footstepsSource.loop = true;
        footstepsSource.playOnAwake = false;
    }

    private void Update()
    {
        Vector3 vel = rb.linearVelocity;
        float horizontalSpeed = new Vector2(vel.x, vel.z).magnitude;
        bool moving = horizontalSpeed > moveThreshold;

        if (moving)
        {
            footstepsSource.pitch = horizontalSpeed > sprintSpeedThreshold ? sprintPitch : walkPitch;
            if (!footstepsSource.isPlaying) footstepsSource.Play();
        }
        else if (footstepsSource.isPlaying)
        {
            footstepsSource.Stop();
        }
    }
}