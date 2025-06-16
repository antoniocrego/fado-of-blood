using UnityEngine;

public class AdamastorLegDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager adamastorCharacter;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        adamastorCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        base.DamageTarget(damageTarget);
        
        
    }
}