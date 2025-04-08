using UnityEngine;

public class IEnemy : MonoBehaviour
{
    public int damageMultiplier = 1; // Damage multiplier on the attacks
    public float detectionRange = 5f; // Range within which the enemy can detect the player
    public float patrolSpeed = 2f; // Speed of the enemy while patrolling
    public float patrolWaitTime = 2f; // Time to wait at each patrol point
    private float patrolWaitTimer = 0f; // Timer to track the wait time at each patrol point
    public GameObject patrolPointsParent; // Parent object containing patrol points
    private Transform[] patrolPoints; // Array of patrol points for the enemy to follow
    private int currentPatrolIndex = 0; // Index of the current patrol point
    private float lastAttackTime; // Time of the last attack
    public SOEnemyAttack[] attacks; // Array of attacks the enemy can perform

    private enum EnemyState
    {
        Idle,
        Patrol,
        Attack,
        Dead
    }
    private EnemyState currentState = EnemyState.Idle; // Current state of the enemy
    private Transform player; // Reference to the player object
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object in the scene
        patrolPoints = new Transform[patrolPointsParent.transform.childCount]; // Initialize the patrol points array
        for (int i = 0; i < patrolPointsParent.transform.childCount; i++)
        {
            patrolPoints[i] = patrolPointsParent.transform.GetChild(i); // Get each child transform as a patrol point
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine(); // Call the state machine to handle enemy behavior
    }

    private void Patrol(){
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        SmoothLookAt(targetPoint, 60f); // Smoothly look at the target point
        targetPoint.position = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z); // Keep the y position of the enemy
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                patrolWaitTimer = 0f;
            }
        }
    }

    private void InCombat(){
        // TODO: Change this so he doesn't turn instantly
        SmoothLookAt(player, 60f); // Smoothly look at the player
    }

    private void Attack(){

    }

    private void StateMachine(){
        if (currentState == EnemyState.Dead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToPlayer <= detectionRange)
                    currentState = EnemyState.Attack;
                else
                    currentState = EnemyState.Patrol;
                break;

            case EnemyState.Patrol:
                Patrol();
                if (distanceToPlayer <= detectionRange)
                    currentState = EnemyState.Attack;
                break;

            case EnemyState.Attack:
                InCombat();
                if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Patrol;
                break;
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