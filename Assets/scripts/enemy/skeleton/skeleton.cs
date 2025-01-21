using UnityEngine;
using System.Collections.Generic;
public class skeleton : EnemyBase
{
    public enum EnemyState { Idle, Roaming, FollowingPlayer, Attacking, TrackingLastSeen }
    public EnemyState currentState;
    public player player; 
    private Vector2 roamTarget;
    private Rigidbody2D rb; 
    public LayerMask obstacleMask; 
    AudioManager audioManager;

    public float moveSpeed = 4f; // Speed at which the enemy moves
    public float damage = 2f; // damage inflicted
    public float roamSpeed = 2f; // Speed for roaming
    public float attackRange = 1.5f; // Distance within which the enemy attacks
    public float detectRange = 7f; // Distance within which the enemy detects the player
    public float attackCooldown = 2f; // Time between attacks
    private float nextAttackTime = 0f; // Timer to track cooldown
    public float MaxHealth = 16f , CurrHealth; // Health of the enemy
    public int scoreValue = 10; // Score value for
    

    public float obstacleAvoidanceRadius = 0.5f; // Radius for obstacle detection

    private Queue<Vector2> playerPath = new Queue<Vector2>(); // Tracks the player's last seen positions
    public int pathMemoryLimit = 15; // How many positions to remember
    public float trackingTimeout = 10f; // Time after which tracking stops
    private float trackingTimer = 0f;

    public float stuckThreshold = 0.1f; // Minimum movement threshold
    public float stuckCheckInterval = 1f; // Time between stuck checks
    private float stuckCheckTimer = 0f;
    private Vector2 lastPosition;


    private float roamDuration; // Duration for roaming
    private float idleDuration; // Duration for idleness
    private float roamTimer = 0f;
    private float idleTimer = 0f;


    public Animator animator;
    private bool isMoving = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<player>();
        }

        rb = GetComponent<Rigidbody2D>();
        idleDuration = Random.Range(4f, 10f); // Random idle duration
        currentState = EnemyState.Idle;
        CurrHealth = MaxHealth;

        // Initialize roam target
        SetNewRoamTarget();
        lastPosition = transform.position;
    }

    void Update()
    {   
        animator.SetFloat("horizontal", roamTarget.x);
        animator.SetFloat("vertical", roamTarget.y);
        animator.SetBool("isMoving", isMoving);

        if (player == null || rb == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState(distanceToPlayer);
                break;

            case EnemyState.Roaming:
                HandleRoamingState(distanceToPlayer);
                break;

            case EnemyState.FollowingPlayer:
                HandleFollowingPlayerState(distanceToPlayer);
                break;

            case EnemyState.Attacking:
                HandleAttackingState(distanceToPlayer);
                break;

            case EnemyState.TrackingLastSeen:
                HandleTrackingLastSeenState(distanceToPlayer);
                break;
        }

        CheckIfStuck();
        UpdatePlayerPath();
    }

    void HandleIdleState(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectRange)
        {
            currentState = EnemyState.FollowingPlayer;
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                idleTimer = 0f;
                currentState = EnemyState.Roaming;
                roamTarget = SetNewRoamTarget();
                roamDuration = Random.Range(2f, 8f);
                isMoving = true;
            }

        }
    }

    void HandleRoamingState(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectRange)
        {
            currentState = EnemyState.FollowingPlayer;
        }
        else
        {
            AvoidObstacles(ref roamTarget);

            roamTimer += Time.deltaTime ;

            if (roamTimer >= roamDuration)
            {
                currentState = EnemyState.Idle;
                roamTimer = 0f;
                idleDuration = Random.Range(4f,10f);
                isMoving = false;
            }
            else 
            {   
                rb.MovePosition(rb.position + roamTarget * roamSpeed * Time.deltaTime);
            }
        }
    }

    void HandleFollowingPlayerState(float distanceToPlayer)
    {   
        isMoving = true;

        if (distanceToPlayer > detectRange)
        {
            trackingTimer = 0f;
            currentState = EnemyState.TrackingLastSeen;
        }
        else if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else
        {
            roamTarget = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            AvoidObstacles(ref roamTarget);
            MoveInDirection(roamTarget, moveSpeed);
        }
    }

    void HandleAttackingState(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.FollowingPlayer;
        }
        else
        {
            AttackPlayer();
            animator.SetFloat("relpos" , player.transform.position.x - transform.position.x);
        }
    }

    void HandleTrackingLastSeenState(float distanceToPlayer)
    {
        isMoving = true;

        if (distanceToPlayer <= detectRange)
        {
            currentState = EnemyState.FollowingPlayer;
            return;
        }

        trackingTimer += Time.deltaTime;
        if (trackingTimer >= trackingTimeout || playerPath.Count == 0)
        {
            currentState = EnemyState.Idle;
            rb.velocity = new Vector2(0,0);
            isMoving = false;
            return;
        }

        Vector2 nextPosition = playerPath.Peek();

        roamTarget = (nextPosition - (Vector2)transform.position).normalized;
        AvoidObstacles(ref roamTarget);
        MoveInDirection(roamTarget, moveSpeed);

        if (Vector2.Distance(transform.position, nextPosition) < 0.5f)
        {
            playerPath.Dequeue();
        }
    }

    void MoveInDirection(Vector2 direction, float speed)
    {
        rb.velocity = direction * speed;
    }

    void AvoidObstacles(ref Vector2 moveDirection)
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, obstacleAvoidanceRadius, obstacleMask);
        Vector2 avoidanceVector = Vector2.zero;
        int avoidCount = 0;

        foreach (var obstacle in obstacles)
        {
            Vector2 awayFromObstacle = ((Vector2)transform.position - (Vector2)obstacle.transform.position).normalized * 1.5f;
            avoidanceVector += awayFromObstacle;
            avoidCount++;
        }

        if (avoidCount > 0)
        {
            avoidanceVector /= avoidCount;
            moveDirection = (moveDirection + avoidanceVector).normalized;
        }
    }

    void CheckIfStuck()
    {
        stuckCheckTimer += Time.deltaTime;

        if (stuckCheckTimer >= stuckCheckInterval)
        {
            stuckCheckTimer = 0f;

            if (Vector2.Distance(lastPosition, transform.position) < stuckThreshold)
            {
                MoveToRandomClearPosition();
            }

            lastPosition = transform.position;
        }
    }

    void MoveToRandomClearPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * 2f; // Small random movement
        Vector2 randomTarget = (Vector2)transform.position + randomDirection;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDirection, 2f, obstacleMask);
        if (hit.collider == null)
        {
            rb.velocity = randomDirection * roamSpeed; // Apply small movement instead of teleportation
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            audioManager.PlaySFX(audioManager.skeletonSFX);
            player.TakeDamage(damage);
            animator.SetTrigger("isAttacking");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    Vector2 SetNewRoamTarget()
    {
        Vector2 randomDirection = new Vector2(
            Random.Range(-1, 1),
            Random.Range(-1, 1)
        );

        if(randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.up;
        }

        randomDirection.Normalize();

        return randomDirection;
    }

    void UpdatePlayerPath()
    {
        if (currentState == EnemyState.FollowingPlayer || currentState == EnemyState.Attacking)
        {
            if (playerPath.Count >= pathMemoryLimit)
            {
                playerPath.Dequeue();
            }

            playerPath.Enqueue(player.transform.position);
        }
    }

    public override void TakeDamage(int damage)
    {
        CurrHealth -= damage;

        if (CurrHealth <= 0)
        {   
            animator.SetTrigger("isDead");
            GetComponent<lootbag>().instantiateItem();
            rb.velocity = new Vector2(0,0);
            Destroy(gameObject , animator.GetCurrentAnimatorStateInfo(0).length);
            scoreManager.instance.AddScore(scoreValue);
        }
        else 
        {
            animator.SetTrigger("isHurt");
        }
    }

}