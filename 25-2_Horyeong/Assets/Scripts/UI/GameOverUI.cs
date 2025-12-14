using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnLoadGameButtonPressed()
    {
        Debug.Log("Load Game button pressed");
        //DataManager.Instance.LoadData();

        //if (DataManager.Instance.gameData == null)
        GameManager.instance.SetGameStart();

        PlayerStatus playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerStatus.Heal(playerStatus.GetmaxHp());

        HideGameOver();
    }

    public void OnExitButtonPressed()
    {
        GameManager.Instance.ExitGame();
    }
}
