using System.Collections.Generic;
using UnityEngine;

public class BoltDamage : MonoBehaviour
{
    [SerializeField] Collider hitbox;      
    [SerializeField] int damage = 55;
    [SerializeField] GameObject bloodPrefab;

    readonly HashSet<EnemyHealth> alreadyHit = new();


    public void EnableHitbox()  { alreadyHit.Clear(); hitbox.enabled = true; }
    public void DisableHitbox() { hitbox.enabled = false; }


    public void Hit(EnemyHealth target)
    {
        if (alreadyHit.Add(target)) target.TakeDamage(damage);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth h)) {
            Vector3 point = other.ClosestPoint(transform.position);
            Hit(h);
            SpawnBlood(point);
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
