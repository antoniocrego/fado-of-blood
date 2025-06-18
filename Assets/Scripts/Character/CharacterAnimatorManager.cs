using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{

    protected CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string lastHitAnimation = string.Empty;
    [SerializeField] string hitForwardMedium = "Hit_Forward_Medium_01";
    [SerializeField] string hitBackwardMedium = "Hit_Backward_Medium_01";
    [SerializeField] string hitLeftMedium = "Hit_Left_Medium_01";
    [SerializeField] string hitRightMedium = "Hit_Right_Medium_01";
    public List<string> forwardHitAnimations = new List<string>();
    public List<string> backwardHitAnimations = new List<string>();
    public List<string> leftHitAnimations = new List<string>();
    public List<string> rightHitAnimations = new List<string>();


    public bool applyRootMotion = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("vertical");
        horizontal = Animator.StringToHash("horizontal");
    }

    protected virtual void Start()
    {
        forwardHitAnimations.Add(hitForwardMedium);
        backwardHitAnimations.Add(hitBackwardMedium);
        leftHitAnimations.Add(hitLeftMedium);
        rightHitAnimations.Add(hitRightMedium);
    }


    public string GetRandomAnimation(List<string> animations)
    {
        if (animations.Count == 0) 
        {
            return string.Empty;
        }

        List<string> availableList = new List<string>();
        foreach (string animation in animations)
        {
            if (string.IsNullOrEmpty(animation) || animation == lastHitAnimation) // Avoid repeating the last hit animation
            {
                continue;
            }

            availableList.Add(animation);
        }
        if (availableList.Count == 0 && animations.Count > 0) // If no valid animations are available, return a random one from the original list
        {
            return animations[Random.Range(0, animations.Count)];
        }
        int randomIndex = Random.Range(0, availableList.Count);
        return availableList[randomIndex];
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isMoving, bool isSpriting)
    {
        float horizontalAmount = horizontalValue;
        float verticalAmount = verticalValue;
        if (isSpriting)
        {
            if (character.isLockedOn)
            {
                if (horizontalValue > 0)
                {
                    horizontalAmount = 2;
                }
                else if (horizontalValue < 0)
                {
                    horizontalAmount = -2;
                }
                if (verticalValue > 0)
                {
                    verticalAmount = 2;
                }
                else if (verticalValue < 0)
                {
                    verticalAmount = -2;
                }
            }
            else
            {
                verticalAmount = 2;
            }

        }
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        character.animator.SetBool("isMoving", isMoving);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRun = true, bool hideWeapons = false)
    {   
        if (!character.animator.HasState(2, Animator.StringToHash(targetAnimation)) && !gameObject.CompareTag("Player"))
            return;
        Debug.Log($"Playing target action animation: {targetAnimation} for character: {character.name}");
        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;

        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRotate = canRotate;

        character.characterLocomotionManager.canRun = canRun;
        
        if (hideWeapons && character is PlayerManager player)
        {
            player.playerEquipmentManager.HideWeapons();
        }
    }

    public virtual void PlayTargetAttackActionAnimation(WeaponItem weapon, AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRun = false)
    {
        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        UpdateAnimatorController("coming from PlayTargetAttackActionAnimation", weapon.weaponAnimator);
        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canRun = canRun;
        character.characterSoundFXManager.PlayAttackGruntSFX();
    }

    public void UpdateAnimatorController(string whereItComesFrom, AnimatorOverrideController weaponController)
    {
        character.animator.runtimeAnimatorController = weaponController;
    }
    
    public virtual void EnableCanDoCombo()
    {

    }

    public virtual void DisableCanDoCombo()
    {

    }
}