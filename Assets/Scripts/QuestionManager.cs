using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // ضيفي هذا في الأعلى لو مو مضاف



public class QuestionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questionPanel;
public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    [Header("Question Data")]
    [TextArea] public string question;
    public string[] answers;
    public int correctAnswerIndex;

    [Header("Level Info")]
    public string nextSceneName;
    public bool isLastLevel = false;

    private SC_FPSController playerController;

    void Start()
    {
        if (questionPanel != null)
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
        if (playerController != null)
            playerController.canMove = false;

        questionPanel.SetActive(true);
        questionText.text = question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(i < answers.Length);
            if (i < answers.Length)
                answerButtons[i].GetComponentInChildren<Text>().text = answers[i];
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnAnswerSelected(int index)
    {
        bool isCorrect = index == correctAnswerIndex;
        GameProgressTracker.SetAnswerResult(isCorrect);

        if (isCorrect)
        {
            if (isLastLevel)
            {
                if (GameProgressTracker.AllAnswersCorrect())
                    questionText.text = "أحسنت! استعد لاسترجاع الكاتانا!";
                else
                    questionText.text = "أخطأت في أحد الأسئلة السابقة... لا يمكنك استعادة الكاتانا.";

                GameProgressTracker.ResetProgress();
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            questionText.text = "إجابتك خاطئة... حاول مجددًا!";
        }

        Invoke(nameof(CloseQuestion), 2f);
    }

    void CloseQuestion()
    {
        questionPanel.SetActive(false);
        if (playerController != null)
        {
            playerController.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
