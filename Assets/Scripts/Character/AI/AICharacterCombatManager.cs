using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float minimumDetectionAngle = -35f;
    [SerializeField] private float maximumDetectionAngle = 35f;

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
                float angle = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                if (angle < maximumDetectionAngle && angle > minimumDetectionAngle)
                {
                    // TODO: voltar aqui e mudar targetCharacter para o lockOnTransform
                    Vector3 increasedYAICharacterPosition = new Vector3(aiCharacter.transform.position.x, aiCharacter.transform.position.y + 0.8f, aiCharacter.transform.position.z);
                    Vector3 increasedYTargetCharacterPosition = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y + 0.8f, targetCharacter.transform.position.z);
                    if (!Physics.Linecast(increasedYAICharacterPosition, increasedYTargetCharacterPosition, LayerMask.GetMask("Default")))
                    {
                        Debug.DrawLine(increasedYAICharacterPosition, increasedYTargetCharacterPosition, Color.red);
                        aiCharacter.characterCombatManager.currentTarget = targetCharacter;
                    }
                }

            }
        }
    }
}