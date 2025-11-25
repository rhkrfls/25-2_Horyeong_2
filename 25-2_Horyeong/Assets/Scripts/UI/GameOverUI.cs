using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    static public GameOverUI instance;

    private void Awake()    // 객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // DontDestroyOnLoad(); 로 파괴 안되도록 막음
        }
        else
            Destroy(this.gameObject);
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
        DataManager.Instance.LoadData();
        GameManager.Instance.LoadGame();
        HideGameOver();
        //SceneController.instance.LoadScene("LoadGameScene");
    }

    public void OnExitButtonPressed()
    {
        GameManager.Instance.ExitGame();
    }
}
