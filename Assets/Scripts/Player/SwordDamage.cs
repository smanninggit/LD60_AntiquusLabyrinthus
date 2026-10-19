using UnityEngine;
using System.Collections.Generic;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] DoAttack attack;
    [SerializeField] GameObject bloodPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth h)) {
            Vector3 point = other.ClosestPoint(transform.position);
            attack.Hit(h);
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