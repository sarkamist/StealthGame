using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }

    private float totalDistance;
    private float totalTime;

    private Text distanceText;
    private Text timeText;

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
    }

    private void OnDistanceChange(float positionDelta)
    {
        totalDistance += positionDelta;
    }
}
