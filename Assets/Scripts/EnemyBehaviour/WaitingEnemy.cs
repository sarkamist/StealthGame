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
    private PlayerDetector playerDetector;
    private WaitingEnemyStates currentState;
    private Transform originPoint;
    private Transform currentTarget;
    private bool isExposingPlayer = false;
    private float currentExposureTime = 0;

    [SerializeField]
    public float RotationSpeed = 1.0f;
    public float MaxExposureTime = 1.0f;
    public float ChaseSpeed = 4.0f;
    public float ReachDistance = 0.1f;
    public float AlignmentThreshold = 5f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerDetector = GetComponentInChildren<PlayerDetector>();
        currentState = WaitingEnemyStates.Wait;
        originPoint = new GameObject($"{name}Origin").transform;
        originPoint.SetParent(GameObject.Find("Waypoints").transform);
        originPoint.position = new Vector3(rigidbody.position.x, rigidbody.position.y, 0);
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
                if (!isExposingPlayer && playerDetector.IsPlayerDetected)
                {
                    isExposingPlayer = true;
                }
                else if (isExposingPlayer && !playerDetector.IsPlayerDetected)
                {
                    isExposingPlayer = false;
                    currentExposureTime = 0;
                }

                if (isExposingPlayer)
                {
                    currentExposureTime += Time.fixedDeltaTime;
                    if (currentExposureTime >= MaxExposureTime)
                    {
                        Debug.Log("change to chase!");
                        currentTarget = playerDetector.DetectedPlayer;
                        ChangeState(WaitingEnemyStates.Chase);
                    }
                }
                else
                {
                    rigidbody.transform.Rotate(0, 0, RotationSpeed);
                }

                break;
            case WaitingEnemyStates.Chase:
                MoveToCurrentTarget(ChaseSpeed);

                if (Vector2.Distance(currentTarget.position, rigidbody.position) > (playerDetector.DetectionRange * 1.5f))
                {
                    ChangeState(WaitingEnemyStates.Returning);
                }

                break;
            case WaitingEnemyStates.Returning:
                MoveToCurrentTarget(ChaseSpeed);

                if (Vector2.Distance((Vector2)currentTarget.position, rigidbody.position) <= ReachDistance)
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
                isExposingPlayer = false;
                currentExposureTime = 0;

                break;
            case WaitingEnemyStates.Chase:
                break;
            case WaitingEnemyStates.Returning:
                rigidbody.linearVelocity = Vector2.zero;

                break;
        }

        currentState = newState;
        switch (currentState)
        {
            case WaitingEnemyStates.Wait:
                break;
            case WaitingEnemyStates.Chase:
                Debug.Log("changing to chase!");
                break;
            case WaitingEnemyStates.Returning:
                currentTarget = originPoint;

                break;
        }
    }

    private void MoveToCurrentTarget(float speed)
    {
        if (currentTarget == null) return;
        Debug.Log("moving!");

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