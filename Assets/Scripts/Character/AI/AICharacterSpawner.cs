using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;
    private AICharacterManager aiCharacterManager;

    private void Awake()
    {

    }
    private void Start()
    {
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter()
    {
        if (characterGameObject != null)
        {
            instantiatedGameObject = Instantiate(characterGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacterManager = instantiatedGameObject.GetComponent<AICharacterManager>();

            if (aiCharacterManager != null)
            {
                WorldAIManager.instance.AddSpawnedCharacter(aiCharacterManager);
            }
        }
    }

    public void ResetCharacter()
    {
        if (instantiatedGameObject == null) return;

        if (aiCharacterManager == null) return;

        instantiatedGameObject.transform.position = transform.position;
        instantiatedGameObject.transform.rotation = transform.rotation;
        aiCharacterManager.health = aiCharacterManager.maxHealth;
        aiCharacterManager.isDead = false;
        // there's conflicting "Empty" states so we have to specify the layer
        aiCharacterManager.animator.Play("Empty", 2, 0f); // reset animation to "Empty" state

        // in case any character's base animation is not "Empty", this function must be implemented to change to that animation
        aiCharacterManager.SetToInitialState();
        aiCharacterManager.characterCombatManager.currentTarget = null; // being in idle state ignores currentTarget, but do it anyways
        aiCharacterManager.navMeshAgent.enabled = false;
        aiCharacterManager.navMeshAgent.enabled = true; // re-enable navmesh agent to reset its state
                                                        // reset ai hp bar ui once its implemented

        if (aiCharacterManager is AIBossCharacterManager boss)
        {
            boss.SoftDeactivateBossFight();
        }
    }
}
