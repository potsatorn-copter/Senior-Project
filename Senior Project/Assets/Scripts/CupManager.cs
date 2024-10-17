using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CupManager : MonoBehaviour
{
    public GameObject[] easyCups; // ถ้วยทั้งหมดสำหรับโหมด Easy
    public GameObject[] normalHardCups; // ถ้วยทั้งหมดสำหรับโหมด Normal และ Hard
    public GameObject[] activeCups; // ถ้วยที่ถูกใช้ในปัจจุบัน

    public GameObject ball; // ลูกบอล
    public TextMeshProUGUI scoreText; // แสดงจำนวนการตอบถูก
    public TextMeshProUGUI finalScoreText; // แสดงคะแนนสุดท้าย
    public TextMeshProUGUI roundText; // แสดงรอบการเล่น
    public Button nextRoundButton; // ปุ่มสำหรับเริ่มรอบถัดไป
    public GameObject finalScoreUI; // GameObject ที่เก็บ UI สำหรับแสดงคะแนนสุดท้าย
    public GameObject Cup4;

    private Transform cupWithBall; // ถ้วยที่มีลูกบอลอยู่
    private bool shuffling = false; // สถานะการสลับถ้วย
    private bool gameStarted = false; // สถานะเริ่มเกม
    private int roundCount = 0; // ตัวนับจำนวนรอบ
    private int correctGuesses = 0; // จำนวนครั้งที่ทายถูก
    private int finalScore = 0; // คะแนนสุดท้าย
    public float liftHeight = 2.0f; // ความสูงที่ถ้วยยกขึ้น
    public float easyShuffleDuration = 1.0f; // ระยะเวลาสลับสำหรับโหมด Easy
    public float normalShuffleDuration = 0.8f; // ระยะเวลาสลับสำหรับโหมด Normal
    public float hardShuffleDuration = 0.6f; // ระยะเวลาสลับสำหรับโหมด Hard
    private float shuffleDuration; // ระยะเวลาสลับที่จะใช้ในปัจจุบัน
    public int easyShuffleTimes = 10; // จำนวนครั้งในการสลับสำหรับ Easy
    public int normalHardShuffleTimes = 12; // จำนวนครั้งในการสลับสำหรับ Normal และ Hard
    private int shuffleTimes; // จำนวนครั้งในการสลับที่จะใช้ในปัจจุบัน

    private Vector3[] initialCupPositions; // ตำแหน่งเริ่มต้นของถ้วยทั้งหมด

    void Start()
    {
        // ซ่อน UI สำหรับคะแนนสุดท้ายตอนเริ่มเกม
        finalScoreUI.SetActive(false);

        // เลือกจำนวนถ้วยและการตั้งค่าอื่นๆ ตามโหมดความยาก
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

        // บันทึกตำแหน่งเริ่มต้นของถ้วย
        initialCupPositions = new Vector3[activeCups.Length];
        for (int i = 0; i < activeCups.Length; i++)
        {
            initialCupPositions[i] = activeCups[i].transform.position;
        }

        nextRoundButton.onClick.AddListener(StartNewRound);
        UpdateUI();
        StartNewRound(); // เริ่มเกมครั้งแรก
    }

    // เริ่มรอบใหม่
    void StartNewRound()
    {
        roundCount++;
        ResetCupsPosition(); // รีเซ็ตตำแหน่งถ้วยทั้งหมด
        UpdateUI();
        StartCoroutine(ShowBallThenCover());
        nextRoundButton.gameObject.SetActive(false); // ซ่อนปุ่มเมื่อเริ่มรอบใหม่
    }

    // รีเซ็ตตำแหน่งถ้วยทั้งหมดกลับไปที่ตำแหน่งเริ่มต้น
    void ResetCupsPosition()
    {
        for (int i = 0; i < activeCups.Length; i++)
        {
            activeCups[i].transform.position = initialCupPositions[i];
        }
    }

    // อัปเดต UI
    void UpdateUI()
    {
        scoreText.text = "Correct Guesses: " + correctGuesses;
        finalScoreText.text = "Final Score: " + finalScore;
        roundText.text = "Round: " + roundCount + "/5";
    }

    // แสดงลูกบอลที่อยู่ใต้ถ้วยในรอบแรก แล้วเอาถ้วยลงมาปิด
    IEnumerator ShowBallThenCover()
    {
        int initialBallPosition = Random.Range(0, activeCups.Length); // เลือกถ้วยที่จะเริ่มมีลูกบอล
        cupWithBall = activeCups[initialBallPosition].transform; // บันทึกถ้วยที่มีลูกบอลอยู่

        // ยกถ้วยขึ้นเพื่อแสดงลูกบอล
        cupWithBall.position += new Vector3(0, liftHeight, 0);
        ball.transform.SetParent(cupWithBall);
        ball.transform.localPosition = new Vector3(0, -4.0f, 0);

        yield return new WaitForSeconds(2); // แสดงบอลให้ผู้เล่นเห็นสักครู่

        // วางถ้วยลงเพื่อปิดลูกบอล
        cupWithBall.position -= new Vector3(0, liftHeight, 0);
        ball.transform.localPosition = new Vector3(0, -0.5f, 0);

        yield return new WaitForSeconds(1); // รออีกสักครู่เพื่อให้ผู้เล่นได้เตรียมตัว

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
                correctGuesses++; // เพิ่มจำนวนครั้งที่ทายถูก
                finalScore = correctGuesses * 2; // คำนวณคะแนนสุดท้าย
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
                nextRoundButton.gameObject.SetActive(true); // แสดงปุ่มเมื่อรอบจบ
            }
            else
            {
                // แสดง UI คะแนนสุดท้าย หลังจาก Reveal เสร็จสิ้น
                StartCoroutine(ShowFinalScoreAfterReveal());
            }
        }
    }

    IEnumerator ShowFinalScoreAfterReveal()
    {
        // รอจนกว่า Reveal จะเสร็จ
        yield return new WaitForSeconds(2);
        Debug.Log("เกมจบแล้ว! คะแนนทั้งหมด: " + finalScore);

        // ส่งคะแนนสุดท้ายไปยัง ScoreManager
        ScoreManager.Instance.SetScoreForScene(5, finalScore); // ซีนที่ 5
        Debug.Log("Score for Scene 5 set in ScoreManager: " + finalScore);

        finalScoreUI.SetActive(true);
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

    public bool IsShuffling()
    {
        return shuffling;
    }
}