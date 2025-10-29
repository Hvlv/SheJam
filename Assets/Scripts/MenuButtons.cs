using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
	[Header("Scene Loading")]
	[SerializeField] private bool useBuildIndex = false; // If true, uses build index; otherwise uses name
	[SerializeField] private int sceneBuildIndex = -1;   // Set a valid index from Build Settings
	[SerializeField] private string sceneName = "";      // Or set a scene name

	// Hook this to the Start button's OnClick
	public void StartGame()
	{
		if (useBuildIndex)
		{
			if (sceneBuildIndex >= 0)
			{
				SceneManager.LoadScene(sceneBuildIndex);
			}
			else
			{
				Debug.LogError("MenuButtons: sceneBuildIndex is not set. Set a valid index in Build Settings.");
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(sceneName))
			{
				SceneManager.LoadScene(sceneName);
			}
			else
			{
				Debug.LogError("MenuButtons: sceneName is empty. Provide a scene name or switch to build index.");
			}
		}
	}

	// Hook this to the Quit button's OnClick
	public void QuitGame()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}


