using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;

    public SceneManagement()
    {
        if (!instance) instance = this;
    }

    public void LoadDefeatScene()
    {
        SceneManager.LoadScene("Defeat");
    }
    public void LoadGame(int level)
    {
        SceneManager.LoadScene("Level0" + level);
    }
}
