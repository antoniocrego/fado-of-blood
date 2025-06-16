using UnityEngine;

public class UndeadHandDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager undeadCharacter;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        undeadCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = undeadCharacter.transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        base.DamageTarget(damageTarget);
    }
}