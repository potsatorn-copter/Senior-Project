using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CupManager : MonoBehaviour
{
    public GameObject[] cups; // ถ้วยทั้งหมด
    public GameObject ball; // ลูกบอล
    public TextMeshProUGUI scoreText; // แสดงจำนวนการตอบถูก
    public TextMeshProUGUI finalScoreText; // แสดงคะแนนสุดท้าย
    public TextMeshProUGUI roundText; // แสดงรอบการเล่น
    public Button nextRoundButton; // ปุ่มสำหรับเริ่มรอบถัดไป
    public GameObject finalScoreUI; // GameObject ที่เก็บ UI สำหรับแสดงคะแนนสุดท้าย

    private Transform cupWithBall; // ถ้วยที่มีลูกบอลอยู่
    private bool shuffling = false; // สถานะการสลับถ้วย
    private bool gameStarted = false; // สถานะเริ่มเกม
    private int roundCount = 0; // ตัวนับจำนวนรอบ
    private int correctGuesses = 0; // จำนวนครั้งที่ทายถูก
    private int finalScore = 0; // คะแนนสุดท้าย
    public float liftHeight = 2.0f; // ความสูงที่ถ้วยยกขึ้น
    public float shuffleDuration = 1.0f; // ระยะเวลาที่ใช้ในการสลับแต่ละครั้ง
    public int shuffleTimes = 8; // จำนวนครั้งในการสลับ

    private Vector3[] initialCupPositions; // ตำแหน่งเริ่มต้นของถ้วยทั้งหมด

    void Start()
    {
        // ซ่อน UI สำหรับคะแนนสุดท้ายตอนเริ่มเกม
        finalScoreUI.SetActive(false);

        // บันทึกตำแหน่งเริ่มต้นของถ้วย
        initialCupPositions = new Vector3[cups.Length];
        for (int i = 0; i < cups.Length; i++)
        {
            initialCupPositions[i] = cups[i].transform.position;
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
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].transform.position = initialCupPositions[i];
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
        int initialBallPosition = Random.Range(0, cups.Length); // เลือกถ้วยที่จะเริ่มมีลูกบอล
        cupWithBall = cups[initialBallPosition].transform; // บันทึกถ้วยที่มีลูกบอลอยู่

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
            int cupA = Random.Range(0, cups.Length);
            int cupB;
            do
            {
                cupB = Random.Range(0, cups.Length);
            } while (cupA == cupB);

            Vector3 cupAPosition = cups[cupA].transform.position;
            Vector3 cupBPosition = cups[cupB].transform.position;

            float elapsedTime = 0;
            while (elapsedTime < shuffleDuration)
            {
                cups[cupA].transform.position = Vector3.Lerp(cupAPosition, cupBPosition, (elapsedTime / shuffleDuration));
                cups[cupB].transform.position = Vector3.Lerp(cupBPosition, cupAPosition, (elapsedTime / shuffleDuration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cups[cupA].transform.position = cupBPosition;
            cups[cupB].transform.position = cupAPosition;
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

            if (cups[selectedIndex].transform == cupWithBall)
            {
                correctGuesses++; // เพิ่มจำนวนครั้งที่ทายถูก
                finalScore = correctGuesses * 2; // คำนวณคะแนนสุดท้าย
                ball.transform.SetParent(null);
                ball.transform.position = cups[selectedIndex].transform.position + new Vector3(0, -6.0f, 0);
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
        finalScoreUI.SetActive(true);
    }

    IEnumerator LiftSelectedCup(int selectedIndex)
    {
        cups[selectedIndex].transform.position += new Vector3(0, 2, 0);
        yield return new WaitForSeconds(1);
    }

    IEnumerator RevealCorrectAndSelectedCup(int selectedIndex)
    {
        cups[selectedIndex].transform.position += new Vector3(0, 2, 0);
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