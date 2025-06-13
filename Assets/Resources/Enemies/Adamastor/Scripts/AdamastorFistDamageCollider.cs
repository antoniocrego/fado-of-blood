using UnityEngine;

public class AdamastorFistDamageCollider : DamageCollider
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
        if(charactersDamaged.Contains(damageTarget)){
            return;
        }

        Debug.Log("Damage target: " + damageTarget);

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.damage = damage;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }
}