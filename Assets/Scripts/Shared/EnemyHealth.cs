using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    int currentHealth;
    public bool notDead = true;

    void Start()
    {
        currentHealth = startingHealth;
        notDead = true;
    }

    public void TakeDamage(int damage){
        Debug.Log(notDead);
        if (notDead){
            currentHealth -= damage;
        }
    }

    void Update(){
        if (currentHealth <= 0 && notDead){
            notDead = false;
            OnDeath();
        }
    }

    void OnDeath(){
        Debug.Log("I DIED!!!!!!!!!!!!!!!!!!!!");
    }
}
