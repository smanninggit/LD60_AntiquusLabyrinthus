using System.Collections.Generic;
using UnityEngine;

public class DoAttack : MonoBehaviour   
{
    [SerializeField] Collider hitbox;      
    [SerializeField] int damage = 10;

    readonly HashSet<EnemyHealth> alreadyHit = new();


    public void EnableHitbox()  { alreadyHit.Clear(); hitbox.enabled = true; }
    public void DisableHitbox() { hitbox.enabled = false; }


    public void Hit(EnemyHealth target)
    {
        if (alreadyHit.Add(target)) target.TakeDamage(damage);
    }
}