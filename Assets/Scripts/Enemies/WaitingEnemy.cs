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

    [SerializeField] private SpriteRenderer alarmSpriteRenderer;
    [SerializeField] private Sprite normalAlarm;
    [SerializeField] private Sprite activeAlarm; 

    public float RotationSpeed = 180.0f;
    public float MaxExposureTime = 1.0f;
    public float ChaseSpeed = 3.0f;
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
        alarmSpriteRenderer.sprite = normalAlarm;
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
                else if (isExposingPlayer && !playerDetector.DetectedPlayer)
                {
                    alarmSpriteRenderer.sprite = normalAlarm;
                    isExposingPlayer = false;
                    currentExposureTime = 0;
                }

                if (isExposingPlayer)
                {
                    if (alarmSpriteRenderer.sprite == normalAlarm) alarmSpriteRenderer.sprite = activeAlarm;
                    else if (alarmSpriteRenderer.sprite == activeAlarm) alarmSpriteRenderer.sprite = normalAlarm;

                    currentExposureTime += Time.fixedDeltaTime;
                    if (currentExposureTime >= MaxExposureTime)
                    {
                        currentTarget = playerDetector.DetectedPlayer;
                        ChangeState(WaitingEnemyStates.Chase);
                    }
                }
                else
                {
                    rigidbody.transform.Rotate(0, 0, RotationSpeed * Time.fixedDeltaTime);
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
                playerDetector.gameObject.SetActive(true);
                rigidbody.linearVelocity = Vector2.zero;
                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case WaitingEnemyStates.Wait:
                alarmSpriteRenderer.sprite = normalAlarm;
                break;
            case WaitingEnemyStates.Chase:
                alarmSpriteRenderer.sprite = activeAlarm;
                break;
            case WaitingEnemyStates.Returning:
                alarmSpriteRenderer.sprite = normalAlarm;
                currentTarget = originPoint;
                playerDetector.gameObject.SetActive(false);
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