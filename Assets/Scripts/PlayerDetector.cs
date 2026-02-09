using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;
public class PlayerDetector : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public LayerMask WhatIsVisible;
    public float DetectionRange;
    public float VisionAngle;

    private void Update()
    {
        if (PlayerDetected())
        {
            Debug.Log("Player detection");
        }
    }

    private bool PlayerDetected()
    {
        Transform playerTransform = null;

        if (PlayerInRange(ref playerTransform))
        {
            if (PlayerInAngle(ref playerTransform))
            {
                PlayerIsVisible(ref playerTransform);
            }
        }

        return (playerTransform != null);
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

        return (hit.collider.transform == target);
    }
}
