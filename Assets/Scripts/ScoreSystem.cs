using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }

    private float totalDistance;
    private float totalTime;

    private Text distanceText;
    private Text timeText;
    private Text scoreText;

    [Header("Scoring")]
    public int MaximumScore = 999;
    public float DistanceThreshold = 75f;
    public float TimeThreshold = 15f;


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
        scoreText.text = $"Score: {CalculateScore(totalDistance, totalTime):000}";
    }

    private void OnDistanceChange(float positionDelta)
    {
        totalDistance += positionDelta;
    }

    public int CalculateScore (float distance, float time)
    {
        return 999;
    }
}
