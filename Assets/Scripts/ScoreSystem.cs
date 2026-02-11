using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }

    private float totalDistance;
    private float totalTime;
    private bool isScoring = false;

    [Header("Scoring")]
    public int MaximumScore = 999;
    public int MinimumScore = 0;
    public float DistanceThreshold = 75f;
    public float TimeThreshold = 20f;
    public float DistanceWeight = 2.5f;
    public float TimeWeight = 1.0f;
    public float PenaltyExponent = 1.45f;
    public float PenaltyPerUnit = 8.0f;

    public float TotalDistance => totalDistance;
    public float TotalTime => totalTime;

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
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayerMovement.OnDistanceChange += OnDistanceChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerMovement.OnDistanceChange -= OnDistanceChange;
    }

    void Update()
    {
        if (isScoring) totalTime += Time.deltaTime;
    }

    private void OnDistanceChange(float positionDelta)
    {
        if (isScoring) totalDistance += positionDelta;
    }

    public int CalculateScore()
    {
        float overDist = Mathf.Max(0f, totalDistance - DistanceThreshold);
        float overTime = Mathf.Max(0f, totalTime - TimeThreshold);

        float distUnits = overDist / 4f;
        float timeUnits = overTime / 2f;

        float weighted = distUnits * DistanceWeight + timeUnits * TimeWeight;

        int penalty = Mathf.RoundToInt(weighted * PenaltyPerUnit);
        float progressive = Mathf.Pow(weighted, PenaltyExponent);

        int score = Mathf.Max(MaximumScore - penalty, MinimumScore);
        return score;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            totalDistance = 0f;
            totalTime = 0f;
            isScoring = true;
        }
        else
        {
            isScoring = false;
        }
    }

    public int SaveScore()
    {
        int storedScore = PlayerPrefs.GetInt("Score");
        int currentScore = CalculateScore();

        if (currentScore >= storedScore)
        {
            PlayerPrefs.SetInt("Score", currentScore);
            return currentScore;
        }
        else
        {
            return storedScore;
        }
    }
}
