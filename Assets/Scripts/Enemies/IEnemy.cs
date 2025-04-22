using UnityEngine;

public class IEnemy : MonoBehaviour
{
    public int damageMultiplier = 1; // Damage multiplier on the attacks
    public float detectionRange = 5f; // Range within which the enemy can detect the player
    public float idleTime = 2f; // Time to wait at each patrol point
    private float idleTimer = 0f; // Timer to track the wait time at each patrol point
    public GameObject patrolPointsParent; // Parent object containing patrol points
    private Transform[] patrolPoints; // Array of patrol points for the enemy to follow
    private int currentPatrolIndex = 0; // Index of the current patrol point
    private float attackTimer = 0f; // Timer to track the attack cooldown
    private float attackCooldown = 2f; // Cooldown time between attacks
    public SOEnemyAttack[] attacks; // Array of attacks the enemy can perform
    private Animator animator; // Reference to the enemy's animator component
    private AnimatorOverrideController animatorOverrideController; // Animator override controller for customizing animations

    private enum EnemyState
    {
        Idle,
        Patrol,
        Combat,
        Dead
    }
    private EnemyState currentState = EnemyState.Idle; // Current state of the enemy
    private Transform player; // Reference to the player object
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object in the scene
        animator = GetComponent<Animator>(); // Get the animator component attached to the enemy
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController); // Create an override controller for the animator
        patrolPoints = new Transform[patrolPointsParent.transform.childCount]; // Initialize the patrol points array
        for (int i = 0; i < patrolPointsParent.transform.childCount; i++)
        {
            patrolPoints[i] = patrolPointsParent.transform.GetChild(i); // Get each child transform as a patrol point
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("State", (int)currentState); // Set the animator state based on the current state
        StateMachine(); // Call the state machine to handle enemy behavior
    }

    private void StateMachine(){
        if (currentState == EnemyState.Dead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState(distanceToPlayer);
                break;

            case EnemyState.Patrol:
                PatrolState(distanceToPlayer);
                break;

            case EnemyState.Combat:
                CombatState(distanceToPlayer);
                break;
        }
    }

    private void IdleState(float distanceToPlayer){
        if (distanceToPlayer <= detectionRange)
            currentState = EnemyState.Combat; // Switch to combat state if player is within range
        else{
            idleTimer += Time.deltaTime; // Increment the wait timer
            if (idleTimer >= idleTime){
                currentState = EnemyState.Patrol; // Switch to patrol state if wait time is over
                idleTimer = 0f; // Reset the wait timer
                idleTime = Random.Range(1.5f, 3f); // Randomize the wait time
            }
        }
    }

    private void PatrolState(float distanceToPlayer){
        PatrolLogic(); // Call the patrol method

        if (distanceToPlayer <= detectionRange)
            currentState = EnemyState.Combat; // Switch to combat state if player is within range
    }
    private void PatrolLogic(){
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        SmoothLookAt(targetPoint, 60f); // Smoothly look at the target point
        targetPoint.position = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z); // Keep the y position of the enemy

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            bool willWait = Random.Range(0f, 1f) < 0.8f; // Randomize whether to wait or not
            if (willWait) // 80% chance to wait
                currentState = EnemyState.Idle; // Switch to idle state
            idleTimer = 0f; // Reset the wait timer

            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Move to the next patrol point
        }
    }

    private void CombatState(float distanceToPlayer){
        CombatLogic(distanceToPlayer);

        if (distanceToPlayer > detectionRange)
            currentState = EnemyState.Patrol;
    }

    private void CombatLogic(float distanceToPlayer){
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) SmoothLookAt(player, 30f); // Very weak tracking of the player while attacking
        else{
            SmoothLookAt(player, 120f); // Smoothly look at the player
            Attack(distanceToPlayer); // Call the attack method
        }
    }

    private void Attack(float distanceToPlayer){
        if (attacks.Length == 0) return; // Return if no attacks are available

        attackTimer += Time.deltaTime; // Increment the attack timer
        if (attackTimer >= attackCooldown) // Check if the cooldown is over
        {
            float angleToPlayer = Vector3.Angle(transform.forward, player.position - transform.position); // Calculate angle to player
            SOEnemyAttack[] usableAttacks = new SOEnemyAttack[attacks.Length]; // Create an array to store usable attacks
            int usableCount = 0; // Counter for usable attacks
            foreach (SOEnemyAttack attack in attacks) // Loop through each attack
            {
                if (attack.CanUse(distanceToPlayer, angleToPlayer)) // Check if the attack can be used
                {
                    usableAttacks[usableCount] = attack; // Add the attack to the usable attacks array
                    usableCount++; // Increment the usable count
                }
            }

            if (usableCount > 0) // If there are usable attacks
            {
                int randomIndex = Random.Range(0, usableCount); // Randomly select an attack
                SOEnemyAttack selectedAttack = usableAttacks[randomIndex]; // Get the selected attack

                animatorOverrideController["Attack"] = selectedAttack.animationClip; // Set the attack animation in the animator override controller
                animator.runtimeAnimatorController = animatorOverrideController; // Apply the override controller to the animator

                animator.SetTrigger("Attack"); // Trigger the attack animation
            }
            attackTimer = 0f;
        }
    }

    void SmoothLookAt(Transform target, float rotationSpeed)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction.magnitude < 0.01f) return; // Avoid zero-length vector errors

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0; // Keep the x rotation at 0 to avoid tilting
        targetRotation.z = 0; // Keep the z rotation at 0 to avoid tilting
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}