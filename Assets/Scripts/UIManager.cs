using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Vector3 topArrowOrigin;
    private Vector3 bottomArrowOrigin;
    private Vector3 topArrowAlternate;
    private Vector3 bottomArrowAlternate;
    private float arrowTimer;

    public Text distanceText;
    public Text timeText;
    public Text scoreText;
    public Transform TopArrow;
    public Transform BottomArrow;

    private void Start()
    {
        topArrowOrigin = new Vector3(TopArrow.position.x, TopArrow.position.y, TopArrow.position.z);
        topArrowAlternate = new Vector3(topArrowOrigin.x, topArrowOrigin.y + 0.25f, topArrowOrigin.z);
        bottomArrowOrigin = new Vector3(BottomArrow.position.x, BottomArrow.position.y, BottomArrow.position.z);
        bottomArrowAlternate = new Vector3(bottomArrowOrigin.x, bottomArrowOrigin.y - 0.25f, bottomArrowOrigin.z);
    }

    void LateUpdate()
    {
        if (ScoreSystem.Instance == null) return;

        if (distanceText != null) distanceText.text = $"Distance: {ScoreSystem.Instance.TotalDistance:0} units";
        if (timeText != null) timeText.text = $"Time: {ScoreSystem.Instance.TotalTime:0.00} sec";
        if (scoreText != null) scoreText.text = $"Score: {ScoreSystem.Instance.CalculateScore():000}";

        arrowTimer += Time.deltaTime;
        if (arrowTimer > 0.25)
        {
            if (TopArrow.position != topArrowOrigin)
            {
                TopArrow.position = topArrowOrigin;
            }
            else
            {
                TopArrow.position = topArrowAlternate;
            }

            if (BottomArrow.position != bottomArrowOrigin)
            {
                BottomArrow.position = bottomArrowOrigin;
            }
            else
            {
                BottomArrow.position = bottomArrowAlternate;
            }

            arrowTimer = 0;
        }
    }
}
