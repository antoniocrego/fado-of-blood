using UnityEngine;

public class SoldierSwordDamageCollider : DamageCollider
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