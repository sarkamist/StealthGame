using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum WaitingEnemyStates
{
    Wait,
    Chase,
    Returning
}

public class WaitingEnemy : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private WaitingEnemyStates currentState;
    [SerializeField]
    private Vector2 originPosition;
    [SerializeField]
    private Vector2 currentTarget;

    [SerializeField]
    public float RotationSpeed = 4.0f;
    public float ChaseSpeed = 4.0f;
    public float ReachDistance = 0.1f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = WaitingEnemyStates.Wait;
        originPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
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
            case WaitingEnemyStates.Wait:
                rigidbody.transform.Rotate(0, 0, RotationSpeed);
                break;
            case WaitingEnemyStates.Chase:
                if (currentTarget != null)
                {
                    Vector2 direction = (currentTarget - rigidbody.position).normalized;
                    Vector2 velocity = direction * ChaseSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                if (Vector2.Distance(currentTarget, rigidbody.position) > 3.0f)
                {
                    Debug.Log("out of reach");
                    ChangeState(WaitingEnemyStates.Returning);
                }
                break;
            case WaitingEnemyStates.Returning:
                if (currentTarget != null)
                {
                    Vector2 direction = (currentTarget - rigidbody.position).normalized;
                    Vector2 velocity = direction * ChaseSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                Debug.Log(Vector2.Distance(currentTarget, rigidbody.position));
                Debug.Log(rigidbody.position);
                if (Vector2.Distance(currentTarget, rigidbody.position) <= ReachDistance)
                {
                    Debug.Log("change state");
                    ChangeState(WaitingEnemyStates.Wait);
                }
                break;
        }
    }

    private void ChangeState(WaitingEnemyStates newState)
    {
        switch (currentState)
        {
            case WaitingEnemyStates.Wait:
                break;
            case WaitingEnemyStates.Chase:
                rigidbody.linearVelocity = Vector2.zero;
                break;
            case WaitingEnemyStates.Returning:
                break;
        }

        currentState = newState;
        switch (currentState)
        {
            case WaitingEnemyStates.Wait:
                break;
            case WaitingEnemyStates.Chase:
                break;
            case WaitingEnemyStates.Returning:
                currentTarget = originPosition;
                break;
        }
    }
    private void OnPlayerDetected(PlayerDetector detector, Transform playerTransform)
    {
        if (currentState == WaitingEnemyStates.Wait && detector.GetComponentInParent<WaitingEnemy>() == this)
        {
            currentTarget = playerTransform.position;
            ChangeState(WaitingEnemyStates.Chase);
        }
    }
}
