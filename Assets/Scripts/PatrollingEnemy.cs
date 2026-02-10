using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum PatrollingEnemyStates
{
    Patrol,
    Chase,
    Returning
}

public class PatrollingEnemy : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private PatrollingEnemyStates currentState;
    private Transform currentTarget;
    private bool isMoving;

    [SerializeField]
    public float PatrolSpeed = 2.0f;
    public float ChaseSpeed = 4.0f;
    public float ReachDistance = 0.1f;
    public List<Transform> Waypoints;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = PatrollingEnemyStates.Patrol;
        currentTarget = Waypoints.FirstOrDefault();
        isMoving = false;
    }

    private void OnEnable()
    {
        PlayerDetector.OnPlayerDetected += OnPlayerDetected;
    }

    private void OnDisable()
    {
        PlayerDetector.OnPlayerDetected -= OnPlayerDetected;
    }

    void FixedUpdate()
    {
        FixedUpdateState();
    }

    private void FixedUpdateState()
    {
        switch (currentState)
        {
            case PatrollingEnemyStates.Patrol:
                if (currentTarget != null)
                {
                    Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;
                    Vector2 velocity = direction * PatrolSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                if (Vector2.Distance(currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    int currentIndex = Waypoints.IndexOf(currentTarget);

                    currentIndex++;
                    if (currentIndex >= Waypoints.Count) currentIndex = 0;
                    currentTarget = Waypoints[currentIndex];
                }
                break;
            case PatrollingEnemyStates.Chase:
                if (currentTarget != null)
                {
                    Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;
                    Vector2 velocity = direction * ChaseSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                if (Vector2.Distance(currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    currentTarget.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
                break;
            case PatrollingEnemyStates.Returning:
                break;
        }
    }

    private void ChangeState(PatrollingEnemyStates newState)
    {
        switch (currentState)
        {
            case PatrollingEnemyStates.Patrol:
                break;
            case PatrollingEnemyStates.Chase:
                break;
            case PatrollingEnemyStates.Returning:
                break;
        }

        currentState = newState;
        switch (currentState)
        {
            case PatrollingEnemyStates.Patrol:
                break;
            case PatrollingEnemyStates.Chase:
                break;
            case PatrollingEnemyStates.Returning:
                break;
        }
    }
    private void OnPlayerDetected(PlayerDetector detector, Transform playerTransform)
    {
        if (currentState != PatrollingEnemyStates.Chase && detector.GetComponentInParent<PatrollingEnemy>() == this)
        {
            currentTarget = playerTransform;
            ChangeState(PatrollingEnemyStates.Chase);
        }
    }
}
