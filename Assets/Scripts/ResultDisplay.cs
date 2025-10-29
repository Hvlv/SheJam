using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultDisplay : MonoBehaviour
{
	[Header("Outcome Objects (two-stage + final)")]
	[SerializeField] private GameObject winFirst;   // First win object (shown on load if all correct)
	[SerializeField] private GameObject winSecond;  // Second win object (shown on key press)
	[SerializeField] private GameObject loseFirst;  // First lose object (shown on load if not all correct)
	[SerializeField] private GameObject loseSecond; // Second lose object (shown on key press)
	[SerializeField] private GameObject finalsObject; // Shown on the second key press (after second object)
	[SerializeField] private bool resetProgressOnShow = true;

	[Header("Menu Actions")]
	[SerializeField] private string mainMenuSceneName = "MainMenu"; // Set your main menu scene name
	[SerializeField] private string postAdvanceSceneName = ""; // Scene to load after Space/Enter
	[SerializeField] private GameObject postAdvanceObject = null; // Object to activate after Space/Enter

	void Awake()
	{
		// Decide once and activate the chosen outcome's first object
		bool allCorrect = GameProgressTracker.AllAnswersCorrect();
		// Ensure all are initially disabled
		if (winFirst != null) winFirst.SetActive(false);
		if (winSecond != null) winSecond.SetActive(false);
		if (loseFirst != null) loseFirst.SetActive(false);
		if (loseSecond != null) loseSecond.SetActive(false);
		if (finalsObject != null) finalsObject.SetActive(false);

		isWinPath = allCorrect;
		advanceStep = 0; // 0 = showing first; 1 = next will show second; 2 = next will show final
		if (isWinPath)
		{
			if (winFirst != null) winFirst.SetActive(true);
		}
		else
		{
			if (loseFirst != null) loseFirst.SetActive(true);
		}

		if (resetProgressOnShow)
		{
			GameProgressTracker.ResetProgress();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if (advanceStep == 0)
			{
				// Activate the second stage for the chosen path (do not hide the first)
				if (isWinPath)
				{
					if (winSecond != null) winSecond.SetActive(true);
				}
				else
				{
					if (loseSecond != null) loseSecond.SetActive(true);
				}
				advanceStep = 1;
			}
			else if (advanceStep == 1)
			{
				// Show the finals object; if not assigned, fall back to optional object/scene
				bool finalShown = false;
				if (finalsObject != null)
				{
					finalsObject.SetActive(true);
					finalShown = true;
				}
				if (!finalShown)
				{
					if (postAdvanceObject != null)
					{
						postAdvanceObject.SetActive(true);
						finalShown = true;
					}
					else if (!string.IsNullOrEmpty(postAdvanceSceneName))
					{
						SceneManager.LoadScene(postAdvanceSceneName);
						finalShown = true;
					}
				}
				advanceStep = 2;
			}
		}
	}

	// Hook this to the Restart button in the result scene
	public void RestartToMainMenu()
	{
		if (!string.IsNullOrEmpty(mainMenuSceneName))
		{
			SceneManager.LoadScene(mainMenuSceneName);
		}
		else
		{
			Debug.LogError("ResultDisplay: mainMenuSceneName is empty. Set it in the inspector.");
		}
	}

	// Hook this to the Quit button in the result scene
	public void Quit()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}

	private bool isWinPath = false;
	private int advanceStep = 0;

}


