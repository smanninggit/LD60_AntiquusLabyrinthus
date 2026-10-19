using UnityEngine;

public class HitPlayer : MonoBehaviour
{

    [SerializeField] int damage = 55;
    [SerializeField] GameObject bloodPrefab;
    [SerializeField] health playerHealth;

    [SerializeField] AudioSource playerHit;
    [SerializeField] AudioClip[] hitClips;

    bool hasHit;

    void OnEnable(){ hasHit = false;}


    void OnTriggerEnter(Collider other)
    {
        if (hasHit || !other.CompareTag("Player")) return;

        if (other.CompareTag("Player")) {
            hasHit = true;
            Vector3 point = other.ClosestPoint(transform.position);
            Hit();
            SpawnBlood(point);
        }
    }

    int lastIndex = -1;

    AudioClip PickClip()
    {
        if (hitClips.Length == 1) return hitClips[0];

        int i;
        do { i = Random.Range(0, hitClips.Length); } while (i == lastIndex);
        lastIndex = i;
        return hitClips[i];
    }

    

    public void ResetHit(){
        hasHit = false;
    }

    public void Hit()
    {
        playerHealth.TakeDamage(damage);
            if (playerHit != null && hitClips.Length > 0)
        {
            AudioClip clip = hitClips[Random.Range(0, hitClips.Length)];
            playerHit.PlayOneShot(PickClip());
        }
    }

    void SpawnBlood(Vector3 point)
    {
        if (bloodPrefab == null) return;
        Quaternion rot = Quaternion.LookRotation(-transform.forward);
        GameObject fx = Instantiate(bloodPrefab, point, rot);
        Destroy(fx, 3f);
    }
}
