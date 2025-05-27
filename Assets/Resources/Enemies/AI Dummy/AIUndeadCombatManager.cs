using UnityEngine;

public class AIUndeadCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
    [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;

    [Header("Damage")]
    [SerializeField] float baseDamage = 10f;
    [SerializeField] float attack01DamageMultiplier = 1f;
    [SerializeField] float attack02DamageMultiplier = 1.5f;

    public void SetAttack01Damage()
    {
        // we could have both calculations here, but since each attack only uses 1 hand, its not worth it
        rightHandDamageCollider.damage = baseDamage * attack01DamageMultiplier;
    }

    public void SetAttack02Damage()
    {
        leftHandDamageCollider.damage = baseDamage * attack02DamageMultiplier;
    }

    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }

}
