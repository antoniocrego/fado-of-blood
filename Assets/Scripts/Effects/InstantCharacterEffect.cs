using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffect : ScriptableObject{
    [Header("Effect ID")]
    public int instantEffectID; // damage = 0, stamina = 1

    public virtual void ProcessEffect(CharacterManager character){
        
    }
}