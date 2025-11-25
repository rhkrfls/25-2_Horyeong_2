using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글턴. singleton 1개... 싱글턴화를 시켜 씬 이동시에도 파괴가 안되도록한다.
    static public GameManager instance;

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
        SceneCount();
        Debug.Log("sceneCount = " + Scene_Count);
        SceneBGM();
        Scene_Count = 0;
        ESC.SetActive(false);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [SerializeField]
    private float GroogyCount = 1f;

    public bool isGroggy = false;
    public bool shouldEvaluate = true;
    public bool isGameOver = false;
    public bool GetIsGameOver() { return isGameOver; } 

    public GameOverUI gameOverPanel;
    public static GameManager Instance = null;
    public static bool isTelapote = false;

    public Image PadeScreen;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float fadeTime;

    [SerializeField]
    private GameObject ESC;

    public static bool GamePause = false;

    public static int Scene_Count = 0;

    private Scene scene;
    [SerializeField]
    private string Lobby_BGM;
    [SerializeField]
    private string IntroScene_BGM;
    [SerializeField]
    private string Main_BGM;

    void Update()
    {
        if (shouldEvaluate && isGroggy)
        {
            StartCoroutine(GroggyPlayer());
            shouldEvaluate = false;
        }

        SceneCount();
    }

    public void ESCBtn(InputAction.CallbackContext context)
    {
        if (GamePause)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public IEnumerator GroggyPlayer()
    {
        yield return new WaitForSeconds(GroogyCount);
        Debug.Log("그로기 풀림!");
        isGroggy = false;
        shouldEvaluate = true;
    }

    private void SceneCount()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "Lobby")
        {
            Scene_Count = 0;
        }
        else if (scene.name == "IntroScene")
        {
            Scene_Count = 1;
        }
        else if (scene.name == "MainScene")
        {
            Scene_Count = 2;
        }
    }

    public void SceneBGM()
    {
        SoundManager.instance.StopAllSoundEffect();
        scene = SceneManager.GetActiveScene();
        if (scene.name == "Lobby")
        {
            SoundManager.instance.PlaySoundBGM(Lobby_BGM);
        }
        else if (scene.name == "IntroScene")
        {
            SoundManager.instance.PlaySoundBGM(IntroScene_BGM);
        }
        else if (scene.name == "MainScene")
        {
            SoundManager.instance.PlaySoundBGM(Main_BGM);
        }
    }

    private void Loding()
    {
        StartCoroutine(ShowEndGame(0, 1));
    }

    private IEnumerator ShowEndGame(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            //f fadeTime으로 나누어서 fadeTime 시간 동안 percent 값이 0 에서 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // 알파값을 start부터 end까지 fadeTime 시간 동안 변화시킨다.
            Color color = PadeScreen.color;
            color.a = Mathf.Lerp(start, end, percent);
            PadeScreen.color = color;

            yield return null;
        }
    }

    public void Pause()
    {
        ESC.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
    }

    public void Resume()
    {
        ESC.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
    }

    public void GetBackScene()
    {
        ESC.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
        SceneManager.LoadScene("Lobby");
    }

    public void LoadGame()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        player.ResetPlayer();

        PlayerStatus playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerStatus.Heal(playerStatus.GetmaxHp());

        ResumeGame();
    }

    public void SetGameStop()
    {
        if (isGameOver) return;

        isGameOver = true;
        
        gameOverPanel.ShowGameOver();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
