using UnityEngine;

public class GameOverUI : MonoBehaviour
{
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

        HideGameOver();
        //SceneController.instance.LoadScene("LoadGameScene");
    }

    public void OnExitButtonPressed()
    {
        Gamemanager.Instance.ExitGame();
    }
}
