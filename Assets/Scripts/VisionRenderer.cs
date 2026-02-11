using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VisionRenderer : MonoBehaviour
{
    private PlayerDetector detector;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    public Color VisionColor = new Color(1f, 0f, 0f, 0.25f);
    public int ArcSegments = 30;

    void Awake()
    {
        detector = GetComponent<PlayerDetector>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mesh.name = "Vision Cone";
        meshFilter.mesh = mesh;

        SetColor(VisionColor);
    }

    void LateUpdate()
    {
        DrawVision();
    }

    private void DrawVision()
    {
        float range = detector.DetectionRange;
        float angle = detector.VisionAngle / 2f;

        int vertexCount = ArcSegments + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[ArcSegments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= ArcSegments; i++)
        {
            float t = i / (float)ArcSegments;
            float nextAngle = Mathf.Lerp(-angle, angle, t);
            Vector3 dir = Quaternion.Euler(0, 0, nextAngle) * Vector3.right;
            vertices[i + 1] = dir.normalized * range;
        }

        int triIndex = 0;
        for (int i = 0; i < ArcSegments; i++)
        {
            triangles[triIndex++] = 0;
            triangles[triIndex++] = i + 1;
            triangles[triIndex++] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    public void SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void ResetColor()
    {
        SetColor(VisionColor);
    }
}