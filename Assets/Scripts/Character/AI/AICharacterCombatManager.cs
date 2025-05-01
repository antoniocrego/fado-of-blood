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
                    if (!Physics.Linecast(aiCharacter.transform.position, targetCharacter.transform.position, LayerMask.GetMask("Default")))
                    {
                        Debug.DrawLine(aiCharacter.transform.position, targetCharacter.transform.position, Color.red);
                        aiCharacter.characterCombatManager.currentTarget = targetCharacter;
                    }
                }

            }
        }
    }
}