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

    [SerializeField]
    public List<Transform> Waypoints;
    public float PatrolSpeed = 2.0f;
    public float ChaseSpeed = 3.0f;
    public float ReachDistance = 0.1f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = PatrollingEnemyStates.Patrol;
        currentTarget = Waypoints.First();
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
        if (currentTarget != null)
        {
            Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;
            Vector2 velocity = direction * PatrolSpeed;
            rigidbody.linearVelocity = velocity;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        switch (currentState)
        {
            case PatrollingEnemyStates.Patrol:
                if (Vector2.Distance(currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    int currentIndex = Waypoints.IndexOf(currentTarget);

                    currentIndex++;
                    if (currentIndex >= Waypoints.Count) currentIndex = 0;

                    currentTarget = Waypoints[currentIndex];
                }
                break;
            case PatrollingEnemyStates.Chase:
                if (Vector2.Distance(currentTarget.position, rigidbody.position) > 3.0f)
                {
                    ChangeState(PatrollingEnemyStates.Returning);
                }
                break;
            case PatrollingEnemyStates.Returning:
                if (Vector2.Distance(currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    ChangeState(PatrollingEnemyStates.Patrol);
                }
                break;
        }
    }

    private void ChangeState(PatrollingEnemyStates newState) {
        switch (currentState)
        {
            case PatrollingEnemyStates.Patrol:
                break;
            case PatrollingEnemyStates.Chase:
                rigidbody.linearVelocity = Vector2.zero;
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
                Transform closestWaypoint = null;
                float closestDistance = float.MaxValue;
                foreach (Transform w in Waypoints)
                {
                    float currentDistance = Vector2.Distance(w.position, rigidbody.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestWaypoint = w;
                    }
                }
                currentTarget = closestWaypoint;
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
