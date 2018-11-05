using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used for General UI Buttons.
/// </summary>
public class SceneManagement : MonoBehaviour
{
	public static SceneManagement instance;

	//Constructores.
	public SceneManagement()
	{
		if (!instance) instance = this;
	}

	//----------------------------------Methods----------------------------------
	public void LoadDefeatScene()
	{
		SceneManager.LoadScene("Defeat");
	}
	public void LoadGame(int level)
	{
		SceneManager.LoadScene("Level0" + level);
	}
	public void ExitGame()
	{
		Application.Quit();
	}
}
