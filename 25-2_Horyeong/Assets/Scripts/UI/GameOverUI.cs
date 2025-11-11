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
<<<<<<< HEAD
        GameManager.Instance.LoadGame();
=======
>>>>>>> origin/Lin

        HideGameOver();
        //SceneController.instance.LoadScene("LoadGameScene");
    }

    public void OnExitButtonPressed()
    {
        GameManager.Instance.ExitGame();
    }
}
