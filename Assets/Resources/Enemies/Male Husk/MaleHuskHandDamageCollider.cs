using UnityEngine;

public class MaleHuskHandDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager maleHuskCharacter;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        maleHuskCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        base.DamageTarget(damageTarget);
        
    }
}