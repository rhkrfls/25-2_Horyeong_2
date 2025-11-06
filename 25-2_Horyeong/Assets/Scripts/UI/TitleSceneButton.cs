using UnityEngine;

public class TitleSceneButton : MonoBehaviour
{
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
