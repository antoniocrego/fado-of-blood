using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour{
    public static WorldCharacterEffectsManager instance;

    [Header("VFX")]
    public GameObject bloodSplatterVFX;

    public GameObject healingFlaskVFX;

    public GameObject bloodPoolVFX;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;
    public TakeBlockedDamageEffect takeBlockedDamageEffect;
    [SerializeField] List<InstantCharacterEffect> instantEffects;

    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }

        GenerateEffectIDs();
    }

    private void GenerateEffectIDs(){
        for(int i = 0; i < instantEffects.Count; i++){
            instantEffects[i].instantEffectID = i;
        }
    }
}