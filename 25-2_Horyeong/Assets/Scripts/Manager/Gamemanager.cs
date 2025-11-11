using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float GroogyCount = 1f;

    public bool isGroggy = false;
    public bool shouldEvaluate = true;
    public bool isGameOver = false;
    public bool GetIsGameOver() { return isGameOver; } 

    public GameOverUI gameOverPanel;
    public static GameManager Instance = null;
    public static bool isTelapote = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (shouldEvaluate && isGroggy)
        {
            StartCoroutine(GroggyPlayer());
            shouldEvaluate = false;
        }
    }

    public IEnumerator GroggyPlayer()
    {
        yield return new WaitForSeconds(GroogyCount);
        Debug.Log("그로기 풀림!");
        isGroggy = false;
        shouldEvaluate = true;
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
