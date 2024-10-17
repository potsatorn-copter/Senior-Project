using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionnaireController : MonoBehaviour
{
    public GameObject[] questions; // Array to hold all the question GameObjects
    public Button nextButton; // Reference to the Next button
    public Button previousButton; // Reference to the Previous button
    private int currentQuestionIndex = 0; // To track the current question index
    public ScoreManagerQuiz scoreManager;  // อ้างอิงถึง ScoreManager
    

    void Start()
    {
        ShowQuestion(currentQuestionIndex);

        // Add listeners to buttons
        nextButton.onClick.AddListener(NextQuestion);
        previousButton.onClick.AddListener(PreviousQuestion);
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < questions.Length - 1)
        {
            SoundManager.instance.Play(SoundManager.SoundName.Click);
            questions[currentQuestionIndex].SetActive(false);
            currentQuestionIndex++;
            ShowQuestion(currentQuestionIndex);
        }
        else
        {
            // ซ่อนคำถามสุดท้ายและแสดงหน้าสุดท้าย
            questions[currentQuestionIndex].SetActive(false);
            // แสดงคะแนนรวมใน TextMeshPro
            int totalScore = scoreManager.GetTotalScore();
            Debug.Log("Final score displayed: " + totalScore);  // ตรวจสอบค่าที่แสดงผลใน TextMeshPro
        }
    }
    
    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            SoundManager.instance.Play(SoundManager.SoundName.Click);
            questions[currentQuestionIndex].SetActive(false);
            currentQuestionIndex--;
            ShowQuestion(currentQuestionIndex);
        }
    }

    void ShowQuestion(int index)
    {
        questions[index].SetActive(true); // Show the question at the specified index
    }
}