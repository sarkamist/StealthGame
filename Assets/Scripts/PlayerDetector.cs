using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public LayerMask WhatIsVisible;
    public float DetectionRange;
    public float VisionAngle;

    public bool IsPlayerDetected;
    public Transform DetectedPlayer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position;
        float halfAngle = VisionAngle * 0.5f;
        int arcSegments = 20;

        Vector3 prev = origin;

        for (int i = 0; i <= arcSegments; i++)
        {
            float t = i / (float) arcSegments;
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * transform.right;

            Vector3 point = origin + dir.normalized * DetectionRange;

            Gizmos.DrawLine(prev, point);
            prev = point;
        }
        Gizmos.DrawLine(prev, origin);

        Gizmos.color = Color.white;
    }

    private void Update()
    {
        PlayerDetected();
    }

    private void PlayerDetected()
    {
        Transform playerTransform = null;

        if (PlayerInRange(ref playerTransform))
        {
            if (PlayerInAngle(ref playerTransform))
            {
                PlayerIsVisible(ref playerTransform);
            }
        }

        DetectedPlayer = playerTransform;
        IsPlayerDetected = (DetectedPlayer != null);
    }

    private bool PlayerInRange(ref Transform playerTransform)
    {
        Collider2D[] playerCollider = Physics2D.OverlapCircleAll(transform.position, DetectionRange, PlayerLayer);

        if (playerCollider.Length > 0)
        {
            playerTransform = playerCollider[0].transform;
        }

        return (playerTransform != null);
    }

    private bool PlayerInAngle(ref Transform playerTransform)
    {
        var angle = GetAngle(playerTransform);

        if (angle > VisionAngle / 2)
        {
            playerTransform = null;
        }

        return (playerTransform != null);
    }

    private float GetAngle(Transform target)
    {
        Vector2 targetDir = target.position - transform.position;
        float angle = Vector2.Angle(targetDir, transform.right);

        return angle;
    }

    private bool PlayerIsVisible(ref Transform playerTransform)
    {
        var isVisible = IsVisible(playerTransform);

        if (!isVisible)
        {
            playerTransform = null;
        }

        return (playerTransform != null);
    }

    private bool IsVisible(Transform target)
    {
        Vector2 dir = target.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, DetectionRange, WhatIsVisible);

        return (hit.collider?.transform == target);
    }
}
