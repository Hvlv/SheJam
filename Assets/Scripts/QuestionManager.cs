// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class QuestionManager : MonoBehaviour
// {
//     [Header("UI Elements")]
//     public GameObject questionPanel; 
//     public TextMeshProUGUI questionText;      
//     public Button[] answerButtons;  

//     [Header("Question Data")]
//     [TextArea] public string question;
//     public string[] answers;         
//     public int correctAnswerIndex;   

//     [Header("Level Info")]
//     public string nextSceneName;    
//     public bool isLastLevel = false;

//     private SC_FPSController playerController;

//     void Start()
//     {
//         questionPanel.SetActive(false);

//         for (int i = 0; i < answerButtons.Length; i++)
//         {
//             int index = i; 
//             answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
//         }
//     }

//     public void ShowQuestion(SC_FPSController controller)
//     {
//         playerController = controller;
//         questionPanel.SetActive(true);

//         questionText.text = question;

//         for (int i = 0; i < answerButtons.Length; i++)
//         {
//             answerButtons[i].gameObject.SetActive(i < answers.Length);

//             if (i < answers.Length)
//             {
//                 var txt = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
//                 if (txt != null)
//                     txt.text = answers[i];
//             }

//             answerButtons[i].interactable = true;
//         }

//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }

//     void OnAnswerSelected(int index)
//     {
//         bool isCorrect = index == correctAnswerIndex;

//         GameProgressTracker.SetAnswerResult(isCorrect);

//         if (isLastLevel)
//         {
//             if (GameProgressTracker.AllAnswersCorrect())
//             {
//                 questionText.text = "Well done! Get ready to reclaim the katana!";
//             }
//             else
//             {
//                 questionText.text = "You made a mistake in one of the previous questions... You cannot reclaim the katana.";
//             }

//             GameProgressTracker.ResetProgress();

//             Invoke(nameof(CloseQuestion), 2f);
//         }
//         else
//         {
//             if (!string.IsNullOrEmpty(nextSceneName))
//             {
//                 SceneManager.LoadScene(nextSceneName);
//             }
//         }
//     }

//     public void ForceCloseQuestion()
//     {
//         if (questionPanel != null)
//             questionPanel.SetActive(false);

//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;

//         if (playerController != null)
//             playerController.canMove = true;
//     }

//     void CloseQuestion()
//     {
//         questionPanel.SetActive(false);

//         if (playerController != null)
//         {
//             playerController.canMove = true;
//             Cursor.lockState = CursorLockMode.Locked;
//             Cursor.visible = false;
//         }
//     }
// }


// QuestionManager.cs





using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI[] answerTexts; 
    [Header("Question Data")]
    [TextArea] public string question;
    public string[] answers;
    public int correctAnswerIndex;

    [Header("Level Info")]
    public string nextSceneName;
    public bool isLastLevel = false;
	public string resultSceneName; // Scene to show win/lose outcome

    private SC_FPSController playerController;

    void Start()
    {
        questionPanel.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    public void ShowQuestion(SC_FPSController controller)
    {
        playerController = controller;

        playerController.canMove = false;
        playerController.canLook = false;

        questionPanel.SetActive(true);
        questionText.text = question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(i < answers.Length);

            if (i < answers.Length && i < answerTexts.Length && answerTexts[i] != null)
            {
                answerTexts[i].text = answers[i];
            }

            answerButtons[i].interactable = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

	void OnAnswerSelected(int index)
    {
        bool isCorrect = index == correctAnswerIndex;
        GameProgressTracker.SetAnswerResult(isCorrect);

        foreach (var btn in answerButtons)
            btn.interactable = false;

		if (isLastLevel)
		{
			// Load result scene which will show win/lose object based on stored progress
			if (!string.IsNullOrEmpty(resultSceneName))
			{
				SceneManager.LoadScene(resultSceneName);
			}
			else
			{
				// Fallback: keep previous inline messaging if no result scene is set
				if (GameProgressTracker.AllAnswersCorrect())
					questionText.text = "Well done! Get ready to reclaim the katana!";
				else
					questionText.text = "You made a mistake in one of the previous questions... You cannot reclaim the katana.";
				Invoke(nameof(CloseQuestion), 2f);
			}
		}
        else
        {
            if (!string.IsNullOrEmpty(nextSceneName))
                SceneManager.LoadScene(nextSceneName);
        }
    }

    public void ForceCloseQuestion()
    {
        if (questionPanel != null)
            questionPanel.SetActive(false);

        RestorePlayerControl();
    }

    void CloseQuestion()
    {
        questionPanel.SetActive(false);
        RestorePlayerControl();
    }

    private void RestorePlayerControl()
    {
        if (playerController != null)
        {
            playerController.canMove = true;
            playerController.canLook = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
