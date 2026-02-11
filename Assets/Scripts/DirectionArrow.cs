using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private RectTransform arrow;
    private Canvas canvas;

    public Transform worldTarget;

    void Awake()
    {
        arrow = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        if (!worldTarget) return;

        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? Camera.main : canvas.worldCamera;
        if (!cam) cam = Camera.main;

        Vector3 targetScreen = cam.WorldToScreenPoint(worldTarget.position);

        Vector3 arrowScreen = RectTransformUtility.WorldToScreenPoint(cam, arrow.position);
        Vector2 direction = targetScreen - arrowScreen;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 180f;

        arrow.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
