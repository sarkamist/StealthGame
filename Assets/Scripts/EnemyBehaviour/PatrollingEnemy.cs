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
    private PlayerDetector playerDetector;
    private PatrollingEnemyStates currentState;
    private Transform currentTarget;

    [SerializeField] private SpriteRenderer alarmSpriteRenderer;
    [SerializeField] private Sprite NormalAlarm;
    [SerializeField] private Sprite ActiveAlarm;

    public List<Transform> Waypoints;
    public float PatrolSpeed = 2.0f;
    public float ChaseSpeed = 3.0f;
    public float ReachDistance = 0.1f;
    public float RotationSpeed = 180f;
    public float AlignmentThreshold = 5f;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerDetector = GetComponentInChildren<PlayerDetector>();
        currentState = PatrollingEnemyStates.Patrol;
        currentTarget = Waypoints.FirstOrDefault();
        alarmSpriteRenderer.sprite = NormalAlarm;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case PatrollingEnemyStates.Patrol:
                MoveToCurrentTarget(PatrolSpeed);

                if (Vector2.Distance(currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    int currentIndex = Waypoints.IndexOf(currentTarget);

                    currentIndex++;
                    if (currentIndex >= Waypoints.Count) currentIndex = 0;

                    currentTarget = Waypoints[currentIndex];
                }

                if (playerDetector.IsPlayerDetected)
                {
                    currentTarget = playerDetector.DetectedPlayer;
                    ChangeState(PatrollingEnemyStates.Chase);
                }

                break;
            case PatrollingEnemyStates.Chase:
                MoveToCurrentTarget(ChaseSpeed);

                if (Vector2.Distance(currentTarget.position, rigidbody.position) > (playerDetector.DetectionRange * 1.5f))
                {
                    ChangeState(PatrollingEnemyStates.Returning);
                }
                break;
            case PatrollingEnemyStates.Returning:
                MoveToCurrentTarget(ChaseSpeed);

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
                alarmSpriteRenderer.sprite = NormalAlarm;
                break;
            case PatrollingEnemyStates.Chase:
                alarmSpriteRenderer.sprite = ActiveAlarm;
                Debug.Log("changing to chase!");
                break;
            case PatrollingEnemyStates.Returning:
                alarmSpriteRenderer.sprite = NormalAlarm;
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

    private void MoveToCurrentTarget(float speed)
    {
        if (currentTarget == null) return;

        Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = rigidbody.transform.eulerAngles.z;
        float nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, RotationSpeed * Time.fixedDeltaTime);

        rigidbody.transform.rotation = Quaternion.Euler(0f, 0f, nextAngle);

        if (Mathf.Abs(Mathf.DeltaAngle(nextAngle, targetAngle)) <= AlignmentThreshold)
        {
            Vector2 velocity = direction * speed;
            rigidbody.linearVelocity = velocity;
        }
        else
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
    }
}
