using UnityEngine;

public enum RandomEnemyStates
{
    Wait,
    Roaming,
    Chase,
    Returning
}

public class RandomEnemy : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private PlayerDetector playerDetector;
    [SerializeField] private RandomEnemyStates currentState;
    private Transform originPoint;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private float waitStart;

    [SerializeField] private SpriteRenderer alarmSpriteRenderer;
    [SerializeField] private Sprite NormalAlarm;
    [SerializeField] private Sprite ActiveAlarm;

    [SerializeField] public float WaitDuration = 1.5f;
    public float RoamingRadius = 4.0f;
    public float RoamingSpeed = 2.0f;
    public float RotationSpeed = 180.0f;
    public float ChaseSpeed = 3.0f;
    public float ReachDistance = 0.1f;
    public float AlignmentThreshold = 5f;

    public RandomEnemyStates CurrentState => currentState;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerDetector = GetComponentInChildren<PlayerDetector>();
        currentState = RandomEnemyStates.Wait;
        waitStart = Time.time;
        originPoint = new GameObject($"{name}Origin").transform;
        originPoint.SetParent(GameObject.Find("Waypoints").transform);
        originPoint.position = new Vector3(rigidbody.position.x, rigidbody.position.y, 0);
        alarmSpriteRenderer.sprite = NormalAlarm;
    }

    void FixedUpdate()
    {
        FixedUpdateState();
    }

    private void FixedUpdateState()
    {
        switch (currentState)
        {
            case RandomEnemyStates.Wait:
                if ((Time.time - waitStart) >= WaitDuration) {
                    ChangeState(RandomEnemyStates.Roaming);
                }

                CheckPlayerDetection();

                break;
            case RandomEnemyStates.Roaming:
                MoveToCurrentTarget(RoamingSpeed);

                if (Vector2.Distance(currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    ChangeState(RandomEnemyStates.Wait);
                }

                CheckPlayerDetection();

                break;
            case RandomEnemyStates.Chase:
                MoveToCurrentTarget(ChaseSpeed);

                float colliderRadius = GetComponent<CircleCollider2D>().radius;
                Collider2D[] playerCollider = Physics2D.OverlapCircleAll(playerDetector.transform.position, colliderRadius, playerDetector.PlayerLayer);

                if (playerCollider.Length > 0)
                {
                    SceneChanger.Instance.OnPlayerCaught();
                }

                if (Vector2.Distance(currentTarget.position, rigidbody.position) > (playerDetector.DetectionRange * 1.25f))
                {
                    ChangeState(RandomEnemyStates.Returning);
                }

                break;
            case RandomEnemyStates.Returning:
                MoveToCurrentTarget(ChaseSpeed);

                if (Vector2.Distance((Vector2)currentTarget.position, rigidbody.position) <= ReachDistance)
                {
                    ChangeState(RandomEnemyStates.Wait);
                }

                break;
        }
    }

    private void ChangeState(RandomEnemyStates newState)
    {
        switch (currentState)
        {
            case RandomEnemyStates.Wait:
                break;
            case RandomEnemyStates.Roaming:
                rigidbody.linearVelocity = Vector2.zero;
                Destroy(GameObject.Find($"{name}RandomPoint"));
                break;
            case RandomEnemyStates.Chase:
                break;
            case RandomEnemyStates.Returning:
                playerDetector.gameObject.SetActive(true);
                rigidbody.linearVelocity = Vector2.zero;

                break;
        }

        currentState = newState;
        switch (currentState)
        {
            case RandomEnemyStates.Wait:
                currentTarget = null;
                waitStart = Time.time;
                break;
            case RandomEnemyStates.Roaming:
                Vector3 center = originPoint.transform.position;
                Vector2 offset = Random.insideUnitCircle * RoamingRadius;

                Transform randomPoint = new GameObject($"{name}RandomPoint").transform;
                randomPoint.SetParent(GameObject.Find("Waypoints").transform);
                randomPoint.position = center + (Vector3) offset;

                currentTarget = randomPoint;
                break;
            case RandomEnemyStates.Chase:
                alarmSpriteRenderer.sprite = ActiveAlarm;
                break;
            case RandomEnemyStates.Returning:
                alarmSpriteRenderer.sprite = NormalAlarm;
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

    private void CheckPlayerDetection()
    {
        if (playerDetector.IsPlayerDetected)
        {
            currentTarget = playerDetector.DetectedPlayer;
            ChangeState(RandomEnemyStates.Chase);
        }
    }
}