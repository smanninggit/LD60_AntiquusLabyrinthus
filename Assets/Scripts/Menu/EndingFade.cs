using UnityEngine;
using UnityEngine.UI;

public class EndingFade : MonoBehaviour
{
    [SerializeField] Image fadeImage;     
    [SerializeField] float maxAlpha = 1f;
    [SerializeField] float smoothSpeed = 2f;      
    [SerializeField] bool reverseDirection;     

    BoxCollider box;
    Transform player;
    float targetAlpha;

    void Awake() => box = GetComponent<BoxCollider>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) player = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        targetAlpha = ProgressAlong(other.transform.position) > 0.5f ? maxAlpha : 0f;
        player = null;
    }

    void Update()
    {
        if (player != null)
            targetAlpha = ProgressAlong(player.position) * maxAlpha;

        Color c = fadeImage.color;
        c.a = Mathf.MoveTowards(c.a, targetAlpha, smoothSpeed * Time.deltaTime);
        fadeImage.color = c;
    }

    // 0 at the entrance side, 1 at the far side, along the zone's local Z axis
    float ProgressAlong(Vector3 worldPos)
    {
        float z = transform.InverseTransformPoint(worldPos).z;
        float half = box.size.z * 0.5f;
        float t = Mathf.InverseLerp(box.center.z - half, box.center.z + half, z);
        return reverseDirection ? 1f - t : t;
    }
}