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
        questionPanel.SetActive(true);

        questionText.text = question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(i < answers.Length);

            if (i < answers.Length)
            {
                var txt = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null)
                    txt.text = answers[i];
            }

            answerButtons[i].interactable = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnAnswerSelected(int index)
    {
        bool isCorrect = index == correctAnswerIndex;

        // حفظ الإجابة مهما كانت صحيحة أو خاطئة
        GameProgressTracker.SetAnswerResult(isCorrect);

        // إذا آخر لفل → تحقق من كل الإجابات وأظهر رسالة
        if (isLastLevel)
        {
            if (GameProgressTracker.AllAnswersCorrect())
            {
                questionText.text = "Well done! Get ready to reclaim the katana!";
            }
            else
            {
                questionText.text = "You made a mistake in one of the previous questions... You cannot reclaim the katana.";
            }

            // إعادة ضبط التقدم بعد الإظهار
            GameProgressTracker.ResetProgress();

            // أغلق البانل بعد ثانيتين
            Invoke(nameof(CloseQuestion), 2f);
        }
        else
        {
            // انتقل للفل التالي مباشرة
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    public void ForceCloseQuestion()
    {
        if (questionPanel != null)
            questionPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
            playerController.canMove = true;
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
