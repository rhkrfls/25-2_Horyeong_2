using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
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

        PlayerController player = FindObjectOfType<PlayerController>();
        player.ResetPlayer();

        Gamemanager.Instance.ResumeGame();

        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        playerStatus.Heal(playerStatus.GetmaxHp());

        HideGameOver();
        //SceneController.instance.LoadScene("LoadGameScene");
    }

    public void OnExitButtonPressed()
    {
        Gamemanager.Instance.ExitGame();
    }
}
