using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyAttack", menuName = "Enemy/EnemyAttack", order = 1)]
public class SOEnemyAttack : ScriptableObject
{
    public float damage;
    public float attackRangeMin; // Minimum range for the attack
    public float attackRangeMax; // Maximum range for the attack
    public float attackAngleMin; // Minimum angle for the attack
    public float attackAngleMax; // Maximum angle for the attack
    public AnimationClip animationClip;
    public AudioClip audioClip;

    public bool CanUse(float distanceToPlayer, float angleToPlayer){
        // Check if the attack can be used based on conditions (e.g., cooldown, range, etc.)
        
        if (distanceToPlayer < attackRangeMin || distanceToPlayer > attackRangeMax || angleToPlayer < attackAngleMin || angleToPlayer > attackAngleMax)
            return false; // Attack is out of range
        // Add more conditions here (e.g., cooldown, player state, etc.)

        return true;
    }

    public float GetDamageValue(){
        return damage * Random.Range(0.8f, 1.2f); // Randomize damage between 80% and 120%
    }
}
