using UnityEngine;
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
        if (scene.name == titleScene)
        {
            Button button = GameObject.Find("Button")?.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnStartGame);
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