using NUnit.Framework;
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

            isMoving = true;
        }
        Debug.Log(Vector2.Distance(targetWaypoint.transform.position, rigidbody.position));
        if (isMoving && Vector2.Distance(targetWaypoint.transform.position, rigidbody.position)<= 0.10f)
        {
            currentWayPoint++;
            if (currentWayPoint >= WayPoints.Count)
            {
                currentWayPoint = 0;
                
            }
            isMoving = false;
        }

    }
}
