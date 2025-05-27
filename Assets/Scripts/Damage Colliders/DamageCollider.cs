using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollider: MonoBehaviour{

    [Header("Damage Collider")]
    [SerializeField] protected Collider damageCollider;

    [Header("Damage")]
    public float damage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered: " + other.gameObject.name);
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();
        Debug.Log("Damage target: " + damageTarget);
        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget){
        if(charactersDamaged.Contains(damageTarget)){
            return;
        }

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.damage = damage;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider(){
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider(){
        damageCollider.enabled = false;
        charactersDamaged.Clear(); // Reset the characters that have been hit, so they can be hit again in the next attack
    }

    protected virtual void Awake()
    {
        
    }
}