using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour{
    
    CharacterManager character;

    protected virtual void Awake(){
        character = GetComponent<CharacterManager>();
    }
    
    //Process instant effects (take damage, heal)
    public virtual void ProcessInstantEffect(InstantCharacterEffect effect){
        effect.ProcessEffect(character);
    }
}