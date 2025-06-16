using UnityEngine;

public class InfanteSwordDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager infanteCharacter;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        infanteCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        base.DamageTarget(damageTarget);
    }
}