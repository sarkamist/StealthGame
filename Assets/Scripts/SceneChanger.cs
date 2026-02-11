using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    private readonly string titleScene = "Title";
    private readonly string gameplayScene = "Gameplay";
    private readonly string defeatScene = "Defeat";
    private readonly string endingScene = "Ending";

    private string CurrentScene => SceneManager.GetActiveScene().name;

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
        VictoryPoint.OnPlayerEnter += OnPlayerVictory;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        VictoryPoint.OnPlayerEnter -= OnPlayerVictory;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == endingScene)
        {
            Text scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
            int currentScore = ScoreSystem.Instance.CalculateScore();
            int bestScore = ScoreSystem.Instance.SaveScore();

            if (currentScore < bestScore)
            {
                scoreText.text = $"Your final score is {currentScore:000}, while your record score is {bestScore:000}";
            }
            else
            {
                scoreText.text = $"Your final score is {currentScore:000}, this is your new record score!";
            }
        }
    }

    public void OnStartGame()
    {
        if (CurrentScene == titleScene)
        {
            SceneManager.LoadScene(gameplayScene);
        }
        else
        {
            SceneManager.LoadScene(titleScene);
        }
    }

    public void OnPlayerCaught()
    {
        if (CurrentScene == gameplayScene)
        {
            SceneManager.LoadScene(defeatScene);
        }
    }

    public void OnPlayerVictory()
    {
        if (CurrentScene == gameplayScene)
        {
            SceneManager.LoadScene(endingScene);
        }
    }

    private void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}