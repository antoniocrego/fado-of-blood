using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AICharacterManager : CharacterManager
{
    [Header("Character Name")]
    public string characterName = "";

    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
    [HideInInspector] public AICharacterStatsManager aiCharacterStatsManager;

    [Header("Navmesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("Current State")]
    [SerializeField] protected AIState currentState;

    [Header("States")]
    public AIStateIdle idleState;
    public AIStatePursue pursueState;
    public AIStateCombat combatState;
    public AIStateAttack attackState;

    protected override void Start()
    {
        base.Start();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        // SOs must be copied to the instance of the object, otherwise they will be shared between all instances.
        idleState = Instantiate(idleState);
        pursueState = Instantiate(pursueState);

        currentState = idleState; // Set the initial state to idle.
    }

    public virtual void SetToInitialState()
    {
        currentState = idleState;
    }

    protected override void Update()
    {
        base.Update();

        aiCharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ProcessStateMachine();
    }

    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if (nextState != null)
        {
            currentState = nextState;
        }

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (aiCharacterCombatManager.currentTarget != null)
        {
            aiCharacterCombatManager.targetDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
            aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetDirection);
            aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
        }

        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }
        animator.SetBool("isMoving", isMoving);
    }

    public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
    {
        yield return base.ProcessDeath(manuallySelectDeathAnimation);

        yield return new WaitForSeconds(2f); // Wait a little before awarding blood drops.

        PlayerManager player = FindFirstObjectByType<PlayerManager>();

        if (player != null)
        {
            player.playerStatsManager.AddBloodDrops(aiCharacterStatsManager.bloodDroppedOnDeath);
        }

        yield return null;
    }
}