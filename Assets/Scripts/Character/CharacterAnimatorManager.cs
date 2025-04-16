using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour{

    CharacterManager character;

    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();
    }
    public void updateAnimatorMovementParameters(float horizontalValue, float verticalValue) 
    {
        Debug.Log("Updated horizontal and vertical values: " + horizontalValue + ", " + verticalValue);
        character.animator.SetFloat("horizontal", horizontalValue, 0.1f, Time.deltaTime); 
        character.animator.SetFloat("vertical", verticalValue, 0.1f, Time.deltaTime);

    }
}