using UnityEngine;
using UnityEngine.UI;

public class TitleSceneButton : MonoBehaviour
{
    public Button loadGameButton;

    private void Start()
    {
        loadGameButton = GameObject.Find("LoadGame").GetComponent<Button>();
        if (DataManager.Instance.gameData == null)
        {
            loadGameButton.image.color = Color.gray;
        }

        else
            loadGameButton.image.color = Color.white;
    }
    public void OnNewGameButtonPressed()
    {
        SceneController.instance.LoadScene("IntroScene");
    }

    public void OnLoadGameButtonPressed()
    {
        Debug.Log("Load Game button pressed");
        SceneController.instance.LoadScene(DataManager.Instance.gameData.lastSceneName);
        //Gamemanager.Instance.LoadGame();
    }

    public void OnOptionButtonPressed()
    {
        Debug.Log("Option button pressed");
        // Implement option menu logic here
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit button pressed");
        Application.Quit();
    }
}
