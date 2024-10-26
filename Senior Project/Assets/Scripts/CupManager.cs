using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CupManager : MonoBehaviour
{
    public GameObject[] easyCups;
    public GameObject[] normalHardCups;
    public GameObject[] activeCups;
    public GameObject ball;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI roundText;
    public Button nextRoundButton;
    public GameObject finalScoreUI;
    public GameObject Cup4;

    private Transform cupWithBall;
    private bool shuffling = false;
    private bool gameStarted = false;
    private int roundCount = 0;
    private int correctGuesses = 0;
    private int finalScore = 0;
    public float liftHeight = 2.0f;
    public float easyShuffleDuration = 1.0f;
    public float normalShuffleDuration = 0.8f;
    public float hardShuffleDuration = 0.6f;
    private float shuffleDuration;
    public int easyShuffleTimes = 10;
    public int normalHardShuffleTimes = 12;
    private int shuffleTimes;

    private Vector3[] initialCupPositions;

    void Start()
    {
        finalScoreUI.SetActive(false);

        if (GameSettings.difficultyLevel == 0) // Easy
        {
            activeCups = easyCups;
            shuffleDuration = easyShuffleDuration;
            shuffleTimes = easyShuffleTimes;
            Cup4.SetActive(false);
        }
        else if (GameSettings.difficultyLevel == 1) // Normal
        {
            activeCups = normalHardCups;
            shuffleDuration = normalShuffleDuration;
            shuffleTimes = normalHardShuffleTimes;
            foreach (GameObject cup in normalHardCups)
            {
                cup.SetActive(true);
            }
        }
        else if (GameSettings.difficultyLevel == 2) // Hard
        {
            activeCups = normalHardCups;
            shuffleDuration = hardShuffleDuration;
            shuffleTimes = normalHardShuffleTimes;
            foreach (GameObject cup in normalHardCups)
            {
                cup.SetActive(true);
            }
        }

        initialCupPositions = new Vector3[activeCups.Length];
        for (int i = 0; i < activeCups.Length; i++)
        {
            initialCupPositions[i] = activeCups[i].transform.position;
        }

        nextRoundButton.onClick.AddListener(StartNewRound);
        UpdateUI();
        StartNewRound();
    }

    void StartNewRound()
    {
        roundCount++;
        ResetCupsPosition();
        UpdateUI();
        StartCoroutine(ShowBallThenCover());
        nextRoundButton.gameObject.SetActive(false);
    }

    void ResetCupsPosition()
    {
        for (int i = 0; i < activeCups.Length; i++)
        {
            activeCups[i].transform.position = initialCupPositions[i];
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Correct Guesses: " + correctGuesses;
        finalScoreText.text = "Final Score: " + finalScore;
        roundText.text = "Round: " + roundCount + "/5";
    }

    IEnumerator ShowBallThenCover()
    {
        int initialBallPosition = Random.Range(0, activeCups.Length);
        cupWithBall = activeCups[initialBallPosition].transform;

        cupWithBall.position += new Vector3(0, liftHeight, 0);
        ball.transform.SetParent(cupWithBall);
        ball.transform.localPosition = new Vector3(0, -4.0f, 0);

        yield return new WaitForSeconds(2);

        cupWithBall.position -= new Vector3(0, liftHeight, 0);
        ball.transform.localPosition = new Vector3(0, -0.5f, 0);

        yield return new WaitForSeconds(1);

        StartCoroutine(ShuffleAnimation());
    }

    IEnumerator ShuffleAnimation()
    {
        shuffling = true;

        for (int i = 0; i < shuffleTimes; i++)
        {
            int cupA = Random.Range(0, activeCups.Length);
            int cupB;
            do
            {
                cupB = Random.Range(0, activeCups.Length);
            } while (cupA == cupB);

            Vector3 cupAPosition = activeCups[cupA].transform.position;
            Vector3 cupBPosition = activeCups[cupB].transform.position;

            float elapsedTime = 0;
            while (elapsedTime < shuffleDuration)
            {
                activeCups[cupA].transform.position = Vector3.Lerp(cupAPosition, cupBPosition, (elapsedTime / shuffleDuration));
                activeCups[cupB].transform.position = Vector3.Lerp(cupBPosition, cupAPosition, (elapsedTime / shuffleDuration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            activeCups[cupA].transform.position = cupBPosition;
            activeCups[cupB].transform.position = cupAPosition;
        }

        ball.transform.SetParent(cupWithBall);
        ball.transform.localPosition = new Vector3(0, -0.5f, 0);

        shuffling = false;
        gameStarted = true;
    }

    public void CheckCup(int selectedIndex)
    {
        if (gameStarted)
        {
            StartCoroutine(LiftSelectedCup(selectedIndex));

            if (activeCups[selectedIndex].transform == cupWithBall)
            {
                correctGuesses++;
                finalScore = correctGuesses * 2;
                ball.transform.SetParent(null);
                ball.transform.position = activeCups[selectedIndex].transform.position + new Vector3(0, -6.0f, 0);
                ball.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                StartCoroutine(RevealCorrectAndSelectedCup(selectedIndex));
            }

            gameStarted = false;
            UpdateUI();

            if (roundCount < 5)
            {
                nextRoundButton.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(ShowFinalScoreAfterReveal());
            }
        }
    }

    IEnumerator ShowFinalScoreAfterReveal()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("เกมจบแล้ว! คะแนนทั้งหมด: " + finalScore);

        ScoreManager.Instance.SetScoreForScene(5, finalScore);
        Debug.Log("Score for Scene 5 set in ScoreManager: " + finalScore);

        finalScoreUI.SetActive(true);

        // เพิ่มระบบดรอปจิ๊กซอว์
        DropJigsawPieceIfEligible(finalScore);
    }

    IEnumerator LiftSelectedCup(int selectedIndex)
    {
        activeCups[selectedIndex].transform.position += new Vector3(0, 2, 0);
        yield return new WaitForSeconds(1);
    }

    IEnumerator RevealCorrectAndSelectedCup(int selectedIndex)
    {
        activeCups[selectedIndex].transform.position += new Vector3(0, 2, 0);
        yield return new WaitForSeconds(1);

        cupWithBall.position += new Vector3(0, 2, 0);
        ball.transform.SetParent(null);
        ball.transform.position = cupWithBall.position + new Vector3(0, -6.0f, 0);
        ball.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(1);
    }

    private void DropJigsawPieceIfEligible(int finalScore)
    {
        if (finalScore >= 8)
        {
            int imageIndex = 0;

            if (GameSettings.difficultyLevel == 0)
            {
                imageIndex = 0;
            }
            else if (GameSettings.difficultyLevel == 1)
            {
                imageIndex = 1;
            }
            else if (GameSettings.difficultyLevel == 2)
            {
                imageIndex = 2;
            }

            JigsawManager.Instance.CollectJigsawPiece(imageIndex, 4);
            Debug.Log($"Jigsaw piece dropped: Scene 5, Difficulty Level: {GameSettings.difficultyLevel}, Image Part 5: {imageIndex}, Final Score: {finalScore}");
        }
        else
        {
            Debug.Log("No jigsaw piece dropped in Scene 5. Final score below threshold.");
        }
    }

    public bool IsShuffling()
    {
        return shuffling;
    }
}