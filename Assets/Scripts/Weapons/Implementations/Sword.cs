using UnityEngine;

public class Sword : WeaponBase
{
    public float swingSpeed = 1.5f;
    public float range = 2.0f;

    
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private AudioClip swordSwingSound;
    [SerializeField] private AudioClip hitSound;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        weaponName = "Sword";
        baseDamage = 15f;
        strength = 5f;
        dexterity = 3f;
        magic = 0f;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    public override void PerformAttack()
    {
        base.PerformAttack();
        
        if (audioSource != null && swordSwingSound != null)
            audioSource.PlayOneShot(swordSwingSound);
            
        if (slashEffect != null)
            slashEffect.Play();
            
        DetectEnemiesInRange();
    }
    
    private void DetectEnemiesInRange()
    {
        Transform playerTransform = transform.root;
        Vector3 position = playerTransform.position;
        Vector3 forward = playerTransform.forward;
        
        Collider[] hitColliders = Physics.OverlapSphere(position + forward * range / 2, range / 2);
        
        foreach (var hitCollider in hitColliders)
        {
            // Skip if it's the player or the weapon itself
            if (hitCollider.transform.IsChildOf(playerTransform) || hitCollider.transform == transform)
                continue;
                
            if (hitCollider.CompareTag("Enemy"))
            {
                OnWeaponHit(hitCollider);
                
                if (audioSource != null && hitSound != null)
                    audioSource.PlayOneShot(hitSound);
            }
        }
    }
}