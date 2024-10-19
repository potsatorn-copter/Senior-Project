using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionnaireController : MonoBehaviour
{
    public GameObject[] questions; // Array to hold all the question GameObjects
    public Button nextButton; // Reference to the Next button
    public Button previousButton; // Reference to the Previous button
    public ScoreManagerQuiz scoreManager;  // อ้างอิงถึง ScoreManager
    public ScoreHistoryManager scoreHistoryManager;
    private int currentQuestionIndex = 0; // To track the current question index
    

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
            // ซ่อนคำถามสุดท้าย
            questions[currentQuestionIndex].SetActive(false);

            // เมื่อถึงคำถามสุดท้าย เรียก FinishQuiz เพื่อบันทึกคะแนน
            FinishQuiz();
            
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
    public void SetupSaveButton(Button saveButton)
    {
        saveButton.onClick.AddListener(() =>
        {
            int finalScore = scoreManager.GetTotalScore();
            scoreHistoryManager.SaveScore(finalScore);
            Debug.Log("คะแนนถูกบันทึก: " + finalScore);
        });
    }
    
    public void FinishQuiz()
    {
        // เรียกคำนวณคะแนนรวมทั้งหมด
        scoreManager.CalculateTotalScore();

        // ดึงคะแนนที่คำนวณได้
        int finalScore = scoreManager.GetTotalScore();

        // บันทึกคะแนนผ่านฟังก์ชัน SaveScore ของ ScoreHistoryManager
        Debug.Log("Final score attempting to save: " + finalScore);
        scoreHistoryManager.SaveScore(finalScore); // บันทึกเฉพาะคะแนนรวมตอนจบควิซเท่านั้น
        Debug.Log("Score saved successfully.");
    }
    
}