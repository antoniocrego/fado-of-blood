using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyAttack", menuName = "Enemy/EnemyAttack", order = 1)]
public class SOEnemyAttack : ScriptableObject
{
    public float damage;
    public float attackRange; // Range from which the enemy can attack the player
    public float attackCooldown; // How long the enemy has to wait before attacking again, useful for not chaining annoying attacks
    public AnimationClip animationClip;
    public AudioClip audioClip;

    public float getDamageValue(){
        return damage * Random.Range(0.8f, 1.2f); // Randomize damage between 80% and 120%
    }
}
