using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }

    private float totalDistance;
    private float totalTime;

    private Text distanceText;
    private Text timeText;
    public Text scoreText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            distanceText = GameObject.Find("Distance").GetComponent<Text>();
            timeText = GameObject.Find("Time").GetComponent<Text>();
            scoreText = GameObject.Find("Score").GetComponent<Text>();
        }
    }

    private void OnEnable()
    {
        PlayerMovement.OnDistanceChange += OnDistanceChange;
    }

    private void OnDisable()
    {
        PlayerMovement.OnDistanceChange -= OnDistanceChange;
    }

    void Update()
    {
        totalTime += Time.deltaTime;

        distanceText.text = $"Distance: {totalDistance:0} units";
        timeText.text = $"Time: {totalTime:0.00} sec";
        scoreText.text = CalculateScore(totalDistance, totalTime).ToString("0.00");
    }

    private void OnDistanceChange(float positionDelta)
    {
        totalDistance += positionDelta;
    }

    public float CalculateScore (float distance, float time)
    {
        const float minDistance = 600f;
        const float minTime = 6f;

        float distanceW = 0.4f;
        float timeW = 0.6f;

        float normDistance = distance / minDistance;
        float normTime = minTime / time;

        float score = distanceW * normDistance + timeW * normTime;
        return score;
    }
    
    public void OnSaveScore()
    {
        score = CalculateScore(totalDistance, totalTime);
        float storedScore = PlayerPrefs.GetFloat("Score");

        if (score < storedScore)
        {
            PlayerPrefs.SetFloat("Score", storedScore)
        }
    }
}
