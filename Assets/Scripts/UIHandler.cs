using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public void LoadScene(int pSceneId)
    {
        SceneManager.LoadScene(pSceneId);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
