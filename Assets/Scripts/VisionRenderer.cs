using UnityEngine;

public class VisionRenderer : MonoBehaviour
{
    [SerializeField] private PlayerDetector detector;
    [SerializeField] private Color visionColor = new Color(1f, 1f, 0f, 0.25f);
    [SerializeField] private int segments = 30;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = visionColor;
        lineRenderer.endColor = visionColor;
    }

    void LateUpdate()
    {
        DrawVision();
    }

    private void DrawVision()
    {
        float range = detector.DetectionRange;
        float angle = detector.VisionAngle/2;

        lineRenderer.positionCount = segments + 2;
        lineRenderer.SetPosition(0, Vector3.zero);

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float currentAngle = Mathf.Lerp(-angle, angle, t);

            Vector3 dir = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;
            Vector3 point = dir.normalized * range;

            lineRenderer.SetPosition(i + 1, point);
        }
    }
}
