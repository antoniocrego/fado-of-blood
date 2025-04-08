using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    public string weaponName;
    public GameObject weaponModel;
    public float baseDamage;
    public float strength;
    public float dexterity;
    public float magic;
    
    public string equipAnimationTrigger = "EquipWeapon";
    public string attackAnimationTrigger = "Attack";
    
    public string WeaponName => weaponName;
    public GameObject WeaponModel => weaponModel;
    
    protected Animator playerAnimator;
    protected bool isEquipped = false;
    
    public virtual void PerformAttack()
    {
        if (!isEquipped) return;
        
        if (playerAnimator != null)
            playerAnimator.SetTrigger(attackAnimationTrigger);
    }
    
    public virtual float CalculateDamage(float playerStrength)
    {
        //simple implementation need to figure out the details
        float damage = baseDamage + (playerStrength * 0.5f);
        
        return damage;
    }
    
    public virtual void EquipWeapon(Transform weaponHolder)
    {
        transform.SetParent(weaponHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        playerAnimator = weaponHolder.GetComponentInParent<Animator>();
        
        isEquipped = true;
        
        if (playerAnimator != null)
            playerAnimator.SetTrigger(equipAnimationTrigger);
    }
    
    protected virtual void OnWeaponHit(Collider enemyCollider)
    {
        // Get enemy health component
        var enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // Get player stats (assuming you have a player stats class)
            var playerStats = GetComponentInParent<PlayerStats>();
            float strength = playerStats != null ? playerStats.Strength : 10f; // Default value if no stats found
            
            // Apply damage
            float damage = CalculateDamage(strength);
            enemyHealth.TakeDamage(damage);
        }
    }
}
