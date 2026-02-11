using UnityEngine;

public enum WaitingEnemyStates
{
    Wait,
    Chase,
    Returning
}

public class WaitingEnemy : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private PlayerDetector playerDetector;
    private WaitingEnemyStates currentState;
    private Transform originPoint;
    private Transform currentTarget;
    private bool isExposingPlayer = false;
    private float currentExposureTime = 0.0f;
    private float alarmFlickerTime = 0.25f;
    private Color arcAlertColor;

    [Header("Behaviour Parameters")]
    public float RotationSpeed = 180.0f;
    public float MaxExposureTime = 0.75f;
    public float ChaseSpeed = 3.0f;
    public float ReachDistance = 0.1f;
    public float AlignmentThreshold = 5f;

    [Header("Alarm Parameters")]
    public SpriteRenderer AlarmSpriteRenderer;
    public VisionRenderer VisionArcRenderer;
    public Sprite NormalAlarm;
    public Sprite ActiveAlarm;

    public WaitingEnemyStates CurrentState => currentState;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerDetector = GetComponentInChildren<PlayerDetector>();
        currentState = WaitingEnemyStates.Wait;
        originPoint = new GameObject($"{name}Origin").transform;
        originPoint.SetParent(GameObject.Find("Waypoints").transform);
        originPoint.position = new Vector3(rigidbody.position.x, rigidbody.position.y, 0);
        AlarmSpriteRenderer.sprite = NormalAlarm;
        arcAlertColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, VisionArcRenderer.VisionColor.a);
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
                    AlarmSpriteRenderer.sprite = NormalAlarm;
                    VisionArcRenderer.ResetColor();
                    isExposingPlayer = false;
                    currentExposureTime = 0;
                }

                if (isExposingPlayer)
                {
                    if (currentExposureTime >= alarmFlickerTime)
                    {
                        alarmFlickerTime += 0.0625f;
                        if (AlarmSpriteRenderer.sprite == NormalAlarm)
                        {
                            AlarmSpriteRenderer.sprite = ActiveAlarm;
                            VisionArcRenderer.SetColor(arcAlertColor);
                        }
                        else if (AlarmSpriteRenderer.sprite == ActiveAlarm)
                        {
                            AlarmSpriteRenderer.sprite = NormalAlarm;
                            VisionArcRenderer.ResetColor();
                        }
                    }

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

                float colliderRadius = GetComponent<CircleCollider2D>().radius;
                Collider2D[] playerCollider = Physics2D.OverlapCircleAll(playerDetector.transform.position, colliderRadius, playerDetector.PlayerLayer);

                if (playerCollider.Length > 0)
                {
                    SceneChanger.Instance.OnPlayerCaught();
                }

                if (Vector2.Distance(currentTarget.position, rigidbody.position) > (playerDetector.DetectionRange * 1.25f))
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
                currentExposureTime = 0.0f;
                alarmFlickerTime = 0.5f;
                VisionArcRenderer.ResetColor();

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
                AlarmSpriteRenderer.sprite = NormalAlarm;

                break;
            case WaitingEnemyStates.Chase:
                AlarmSpriteRenderer.sprite = ActiveAlarm;

                break;
            case WaitingEnemyStates.Returning:
                AlarmSpriteRenderer.sprite = NormalAlarm;
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