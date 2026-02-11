using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text distanceText;
    public Text timeText;
    public Text scoreText;

    void Update()
    {
        if (distanceText != null) distanceText.text = $"Distance: {ScoreSystem.Instance.TotalDistance:0} units";
        if (timeText != null) timeText.text = $"Time: {ScoreSystem.Instance.TotalTime:0.00} sec";
        if (scoreText != null) scoreText.text = $"Score: {ScoreSystem.Instance.CalculateScore():000}";
    }
}
