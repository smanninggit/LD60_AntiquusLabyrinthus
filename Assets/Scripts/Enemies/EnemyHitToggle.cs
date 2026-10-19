using UnityEngine;

public class EnemyHitToggle : MonoBehaviour
{

    [SerializeField] Collider hitbox;  
    [SerializeField] HitPlayer hit;

    public void EnableHitbox()  { hitbox.enabled = true; hit.ResetHit();}
    public void DisableHitbox() { hitbox.enabled = false; }
}
