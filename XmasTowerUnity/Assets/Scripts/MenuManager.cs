using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas GameOverUI;

    public void Start()
    {
        if (GameOverUI)
            ShowGameOverButtons(false);
    }

    public void ShowGameOverButtons(bool value = true)
    {
        GameOverUI.sortingOrder = value ? 10 : -1;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
