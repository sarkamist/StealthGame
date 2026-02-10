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
    [SerializeField]
    private WaitingEnemyStates currentState;
    [SerializeField]
    private Vector2 originPosition;
    [SerializeField]
    private Transform currentTarget;
    private float maxExposureTime = 1;
    private bool isExposingPlayer = false;

    [SerializeField]
    public float RotationSpeed = 4.0f;
    public float ChaseSpeed = 4.0f;
    public float ReachDistance = 0.1f;
    public float ExposureTime = 0;

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
                if (!isExposingPlayer)
                {
                    rigidbody.transform.Rotate(0, 0, RotationSpeed);
                }
                break;
            case WaitingEnemyStates.Chase:
                if (currentTarget != null)
                {
                    Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;
                    Vector2 velocity = direction * ChaseSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                if (Vector2.Distance(currentTarget.position, rigidbody.position) > 3.0f)
                {
                    Debug.Log("out of reach");
                    ChangeState(WaitingEnemyStates.Returning);
                }
                break;
            case WaitingEnemyStates.Returning:
                if (currentTarget != null)
                {
                    Vector2 direction = ((Vector2)currentTarget.position - rigidbody.position).normalized;
                    Vector2 velocity = direction * ChaseSpeed;
                    rigidbody.linearVelocity = velocity;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
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
                break;
            case WaitingEnemyStates.Returning:
                GameObject returnPosition = new GameObject("ReturnPosition");
                returnPosition.transform.position = new Vector3(originPosition.x, originPosition.y, 0);
                currentTarget = returnPosition.transform;
                break;
        }
    }
    private void OnPlayerDetected(PlayerDetector detector, Transform playerTransform)
    {
        if(ExposureTime < maxExposureTime)
        {
            ExposureTime += Time.deltaTime;
        }
        else
        {
            isExposingPlayer = true;
        }

        if (currentState == WaitingEnemyStates.Wait && detector.GetComponentInParent<WaitingEnemy>() == this)
        {
            currentTarget = playerTransform;
            ChangeState(WaitingEnemyStates.Chase);
        }
    }
}