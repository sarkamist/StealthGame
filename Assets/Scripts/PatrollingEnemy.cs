using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
    private int currentWayPoint;
    private bool isMoving;

    [SerializeField]
    public List<GameObject> WayPoints;
    public float Speed = 4.0f; 

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = PatrollingEnemyStates.Patrol;
        currentWayPoint = 0;
        isMoving = false;
    }

    void FixedUpdate()
    {
        GameObject targetWaypoint = WayPoints[currentWayPoint];
        
        if (!isMoving && targetWaypoint !=null)
        {
            Debug.Log("new waypoint");
           Vector2 direction = ((Vector2)targetWaypoint.transform.position - rigidbody.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector2 velocity = direction * Speed;
            rigidbody.linearVelocity = velocity;

<<<<<<< Updated upstream
            isMoving = true;
=======
                    currentIndex++;
                    if (currentIndex >= Waypoints.Count) currentIndex = 0;
                    currentTarget = Waypoints[currentIndex];
                }
                break;
            case PatrollingEnemyStates.Chase:
                if (currentTarget != null)
                {
                    Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;
                    Vector2 velocity = direction * PatrolSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                if (Vector2.Distance(currentTarget.position, rigidbody.position) > 3.0f)
                {
                    ChangeState(PatrollingEnemyStates.Returning);
                }
                break;
            case PatrollingEnemyStates.Returning:
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
                    ChangeState(PatrollingEnemyStates.Patrol);
                }
                break;
>>>>>>> Stashed changes
        }
        Debug.Log(Vector2.Distance(targetWaypoint.transform.position, rigidbody.position));
        if (isMoving && Vector2.Distance(targetWaypoint.transform.position, rigidbody.position)<= 0.10f)
        {
<<<<<<< Updated upstream
            currentWayPoint++;
            if (currentWayPoint >= WayPoints.Count)
            {
                currentWayPoint = 0;
                
            }
            isMoving = false;
        }

=======
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
>>>>>>> Stashed changes
    }
}
