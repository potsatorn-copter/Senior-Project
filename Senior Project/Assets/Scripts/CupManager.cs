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
    public JigsawUIPanel jigsawUIPanel;

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
        scoreText.text = "ทายถูก: " + correctGuesses;
        finalScoreText.text = "คะแนนที่ได้ : " + finalScore;
        roundText.text = "รอบที่เหลือ " + roundCount + "/5";
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
        // ปิดการแสดงผลลูกบอลก่อนเริ่มการสลับ
        ball.SetActive(false);
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

        // การสลับเสร็จสิ้นแล้ว ให้แสดงลูกบอลในแก้วที่ถูกต้อง
        ball.SetActive(true);
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
                // เล่นเสียงเมื่อเลือกถูก
                SoundManager.instance.Play(SoundManager.SoundName.CorrectItem);
            
                correctGuesses++;
                finalScore = correctGuesses * 2;
                ball.transform.SetParent(null);
                ball.transform.position = activeCups[selectedIndex].transform.position + new Vector3(0, -7.0f, 0);
                ball.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                // เล่นเสียงเมื่อเลือกผิด
                SoundManager.instance.Play(SoundManager.SoundName.WrongItem);
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

        // เรียกใช้การดรอปจิ๊กซอว์ (หรือแสดง finalScoreUI ถ้าไม่มีการดรอปจิ๊กซอว์)
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
        ball.transform.position = cupWithBall.position + new Vector3(0, -7.0f, 0);
        ball.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(1);
    }

    private void DropJigsawPieceIfEligible(int finalScore)
    {
        if (finalScore >= 8)
        {
            int imageIndex = GameSettings.difficultyLevel;
            JigsawManager.JigsawImage jigsawImage = JigsawManager.Instance.jigsawImages[imageIndex];
            JigsawManager.JigsawPiece droppedPiece = jigsawImage.jigsawPieces[4]; // ชิ้นส่วนที่ index 4

            // ตรวจสอบว่าชิ้นส่วนนั้นถูกเก็บไปแล้วหรือไม่
            if (!droppedPiece.isCollected)
            {
                // เก็บชิ้นส่วนถ้ายังไม่ถูกเก็บ
                JigsawManager.Instance.CollectJigsawPiece(imageIndex, 4);

                if (jigsawUIPanel != null)
                {
                    Sprite jigsawSprite = droppedPiece.pieceSprite;
                    jigsawUIPanel.ShowJigsawUIPanel(jigsawSprite, "คุณได้รับชิ้นส่วนจิ๊กซอว์ใหม่!");
                    StartCoroutine(ShowEndGamePanelWithDelay(jigsawUIPanel));
                }
                else
                {
                    Debug.LogWarning("JigsawUIPanel is not found in the scene.");
                    ShowEndGamePanel();
                }
            }
            else
            {
                // แสดงข้อความแจ้งเตือนว่าชิ้นส่วนนี้ถูกเก็บไปแล้ว
                Debug.Log("Jigsaw piece already collected - no UI shown.");
                ShowEndGamePanel();
            }
        }
        else
        {
            // กรณีที่คะแนนต่ำกว่า 8 ให้แสดง finalScoreUI โดยไม่ต้องรอ jigsawUIPanel
            Debug.Log("No jigsaw piece dropped in Scene 5. Final score below threshold.");
            ShowEndGamePanel();
        }
    }
    
    private IEnumerator ShowEndGamePanelWithDelay(JigsawUIPanel jigsawUIPanel)
    {SoundManager.instance.Play(SoundManager.SoundName.WinSound);
        yield return new WaitForSeconds(3f); // รอให้ JigsawUIPanel แสดงครบ 3 วินาที
        if (jigsawUIPanel != null)
        {
            jigsawUIPanel.HideJigsawUIPanel();
        }
        yield return new WaitForSeconds(0.5f); // รออีก 0.5 วินาที
        ShowEndGamePanel();
    }

    private void ShowEndGamePanel()
    {
        if (finalScoreUI != null && !finalScoreUI.activeSelf)
        {
            finalScoreUI.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลา
        }
    }
    
    public bool IsShuffling()
    {
        return shuffling;
    }
}