// using UnityEngine;

// public static class GameProgressTracker
// {
//     private static bool[] answers = new bool[3];
//     private static int currentIndex = 0;

//     public static void SetAnswerResult(bool isCorrect)
//     {
//         if (currentIndex < answers.Length)
//             answers[currentIndex++] = isCorrect;
//     }

//     public static bool AllAnswersCorrect()
//     {
//         foreach (bool correct in answers)
//         {
//             if (!correct) return false;
//         }
//         return true;
//     }

//     public static void ResetProgress()
//     {
//         for (int i = 0; i < answers.Length; i++)
//             answers[i] = false;
//         currentIndex = 0;
//     }
// }

// GameProgressTracker.cs




using UnityEngine;

public static class GameProgressTracker
{
    private static bool[] answers = new bool[3]; 
    private static int currentIndex = 0;

    public static void SetAnswerResult(bool isCorrect)
    {
        if (currentIndex < answers.Length)
            answers[currentIndex++] = isCorrect;
    }

    public static bool AllAnswersCorrect()
    {
        foreach (bool correct in answers)
        {
            if (!correct) return false;
        }
        return true;
    }

    public static void ResetProgress()
    {
        for (int i = 0; i < answers.Length; i++)
            answers[i] = false;
        currentIndex = 0;
    }
}
