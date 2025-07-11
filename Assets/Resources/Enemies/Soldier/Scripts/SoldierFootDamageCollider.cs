using UnityEngine;

public class SoldierFootDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager soldierCharacter;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        soldierCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        base.DamageTarget(damageTarget);
    }
}