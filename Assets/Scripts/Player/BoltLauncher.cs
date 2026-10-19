using UnityEngine;

public class BoltLauncher : MonoBehaviour
{
    [SerializeField] GameObject boltPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float boltSpeed = 15f;
    [SerializeField] float lifetime = 5f;
    public bool isLoaded = false;

    GameObject currentBolt;

    public void Awake(){
        isLoaded = false;
    }

    public void SpawnBolt()
    {
        if (currentBolt != null) Destroy(currentBolt);

        currentBolt = Instantiate(boltPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);

        if (currentBolt.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;  
        if (currentBolt.TryGetComponent(out Collider col)) col.enabled = false; 

        isLoaded = true;
    }

    public void ShootBolt()
    {
        if (currentBolt == null) return;


        currentBolt.transform.SetParent(null);

        if (currentBolt.TryGetComponent(out Collider col)) col.enabled = true;
        if (currentBolt.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(spawnPoint.up * boltSpeed, ForceMode.VelocityChange);
        }
        isLoaded = false;

        Destroy(currentBolt, lifetime);
        currentBolt = null;
    }
}