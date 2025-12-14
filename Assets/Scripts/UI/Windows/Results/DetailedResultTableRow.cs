using TMPro;
using UnityEngine;

/// <summary>
/// Класс строки таблицы подробных результатов
/// </summary>
public class DetailedResultTableRow : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text givenAnswerText;
    [SerializeField] private TMP_Text correctAnswerText;

    /// <summary>
    /// Задать значения строки таблицы подробных результатов
    /// </summary>
    /// <param name="result"></param>
    public void SetupRow(DetailedResult result)
    {
        int index = result.Index;
        string givenAnswer = result.GivenAnswerName;
        string correctAnswer = result.CorrectAnswerName;

        nameText.text = $"Свинья #{index + 1}";

        if (givenAnswer == correctAnswer)
        {
            givenAnswerText.text = $"<style=\"Positive\">{givenAnswer}</style>";
        }
        else
        {
            givenAnswerText.text = $"<style=\"Negative\">{givenAnswer}</style>";
        }

        correctAnswerText.text = correctAnswer;
    }
}
