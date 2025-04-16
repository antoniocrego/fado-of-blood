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

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion=true, bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;

        character.canMove = canMove; 
        character.canRotate = canRotate; 


    }
}