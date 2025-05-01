using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
    {
        switch (attackingCharacter)
        {
            case CharacterGroup.Player:
                return targetCharacter == CharacterGroup.Enemy || targetCharacter == CharacterGroup.Neutral;
            case CharacterGroup.Friendly:
                return false; // Friendlies cannot be damaged and cannot damage.
            case CharacterGroup.Enemy:
                return targetCharacter == CharacterGroup.Player;
            case CharacterGroup.Neutral:
                return targetCharacter == CharacterGroup.Player;
        }

        return false;
    }
    
    public float GetAngleOfTarget(Transform transform, Vector3 targetDirection)
    {
        targetDirection.y = 0;
        float angle = Vector3.Angle(transform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(transform.forward, targetDirection);

        if (cross.y < 0) angle = -angle;
        
        return angle;
    }
}