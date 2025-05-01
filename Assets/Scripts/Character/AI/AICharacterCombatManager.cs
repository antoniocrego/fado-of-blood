using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Viewable Angle")]
    public float viewableAngle = 35f;
    public Vector3 targetDirection;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 10f;
    public float minimumFOV = -35f;
    public float maximumFOV = 35f;

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
                        PivotTowardsTarget(aiCharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return;

        // this should be working, but it is not working
        if (viewableAngle >= 60 && viewableAngle <= 110){
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 L", true);
        }
        else if (viewableAngle <= -60 && viewableAngle >= -110){
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 R", true);
        }
    }
}