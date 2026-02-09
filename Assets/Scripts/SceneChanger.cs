using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    private readonly string titleScene = "Title";
    private readonly string gameplayScene = "Gameplay";
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
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}