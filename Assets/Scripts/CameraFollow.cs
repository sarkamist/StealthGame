using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public TilemapCollider2D MapBounds;
    public float SmoothSpeed = 0.5f;

    private float xMin, xMax, yMin, yMax;
    private float camOrthSize, camHalfWidth;

    private void Start()
    {
        xMin = MapBounds.bounds.min.x;
        xMax = MapBounds.bounds.max.x;
        yMin = MapBounds.bounds.min.y;
        yMax = MapBounds.bounds.max.y;
        camOrthSize = Camera.main.orthographicSize;
        camHalfWidth = camOrthSize * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            float camX = Mathf.Clamp(target.position.x, xMin + camHalfWidth, xMax - camHalfWidth);
            float camY = Mathf.Clamp(target.position.y, yMin + camOrthSize, yMax - camOrthSize);

            Vector3 smoothPos = Vector3.Lerp(transform.position, new Vector3(camX, camY, transform.position.z), SmoothSpeed);
            transform.position = smoothPos;
        }
    }
}
