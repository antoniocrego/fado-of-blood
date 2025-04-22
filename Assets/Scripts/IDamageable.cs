using UnityEngine;

public class IDamageable : MonoBehaviour{
    public int health = 100; // Health

    public void TakeDamage(int damage){
        health -= damage; // Reduce health by damage amount
        Debug.Log("Damage taken: " + damage + ", Health left: " + health);
        if (health <= 0){
            Die(); // Call die method if health is 0 or less
        }
    }

    private void Die(){
        // Handle death logic here (e.g., play animation, destroy object, etc.)
        Debug.Log("Enemy has died.");
    }
}