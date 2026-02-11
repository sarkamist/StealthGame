using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text DistanceText;
    public Text TimeText;
    public Text ScoreText;

    void Update()
    {
        if (DistanceText != null) DistanceText.text = $"Distance: {ScoreSystem.Instance.TotalDistance:0} units";
        if (TimeText != null) TimeText.text = $"Time: {ScoreSystem.Instance.TotalTime:0.00} sec";
        if (ScoreText != null) ScoreText.text = $"Score: {ScoreSystem.Instance.CalculateScore():000}";
    }
}
