using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    private bool isMoving;
    private Vector2 lastPosition;

    private int score = 2000;

    [SerializeField]
    private float Speed = 5.0f;
    public bool IsMoving => isMoving;

    public static Action<float> OnDistanceChange;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        lastPosition = rigidbody.position;
    }

    void Update()
    {
        float positionDelta = Vector2.Distance(rigidbody.position, lastPosition);

        if (positionDelta > 0.01f)
        {
            OnDistanceChange?.Invoke(positionDelta);
            lastPosition = rigidbody.position;
        }
    }

    private void OnMove(InputValue value)
    {
        var moveDir = value.Get<Vector2>();
        Debug.Log(moveDir);

        Vector2 velocity = moveDir * Speed;
        rigidbody.linearVelocity = velocity;

        isMoving = (velocity.magnitude > 0.01f);

        if (isMoving)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void OnSaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
        score = PlayerPrefs.GetInt("Score");
    }
}
