using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0f;


    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Pivot")]
    public bool canPivot = true;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 10f;
    public float minimumFOV = -35f;
    public float maximumFOV = 35f;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 5f;

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter){
        if (currentTarget != null) return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, LayerMask.GetMask("Character"));

        foreach (Collider collider in colliders)
        {
            CharacterManager targetCharacter = collider.GetComponent<CharacterManager>();

            if (targetCharacter == null || targetCharacter==aiCharacter || targetCharacter.isDead) continue;

            if (WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfTarget = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                if (angleOfTarget < maximumFOV && angleOfTarget > minimumFOV)
                {
                    // TODO: voltar aqui e mudar targetCharacter para o lockOnTransform
                    Vector3 increasedYAICharacterPosition = new Vector3(aiCharacter.transform.position.x, aiCharacter.transform.position.y + 0.8f, aiCharacter.transform.position.z);
                    Vector3 increasedYTargetCharacterPosition = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y + 0.8f, targetCharacter.transform.position.z);
                    if (!Physics.Linecast(increasedYAICharacterPosition, increasedYTargetCharacterPosition, LayerMask.GetMask("Default")))
                    {
                        targetDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetDirection);
                        aiCharacter.characterCombatManager.currentTarget = targetCharacter;

                        if (canPivot)
                            PivotTowardsTarget(aiCharacter);
                    }
                }
            }
        }
    }

    public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return;

        if (viewableAngle >= 20 && viewableAngle <= 60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 45 R", true);
        }
        else if (viewableAngle >= 60 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 R", true);
        }
        else if (viewableAngle >= 110 && viewableAngle <= 145)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 135 R", true);
        }
        else if (viewableAngle >= 145 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 180 R", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= -60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 45 L", true);
        }
        else if (viewableAngle <= -60 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 L", true);
        }
        else if (viewableAngle <= -110 && viewableAngle >= -145)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 135 L", true);
        }
        else if (viewableAngle <= -145 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 180 L", true);
        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isMoving){
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhileAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget == null) return;

        if (!aiCharacter.aiCharacterLocomotionManager.canRotate) return;

        if (!aiCharacter.isPerformingAction) return;

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
            targetDirection = aiCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }

    public void HandleActionRecovery(AICharacterManager aiCharacter){
        if (actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
        else if (actionRecoveryTimer < 0)
        {
            actionRecoveryTimer = 0;
        }
    }
}