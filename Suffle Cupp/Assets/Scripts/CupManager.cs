using UnityEngine;
using System.Collections;

public class CupManager : MonoBehaviour
{
    public GameObject[] cups; // ถ้วยทั้งหมด
    public GameObject ball; // ลูกบอล
    private int ballPosition; // ตำแหน่งที่ลูกบอลซ่อนอยู่
    private bool shuffling = false; // สถานะการสลับถ้วย
    private bool gameStarted = false; // สถานะเริ่มเกม
    public float liftHeight = 2.0f; // ความสูงที่ถ้วยยกขึ้น
    public float shuffleDuration = 1.0f; // ระยะเวลาที่ใช้ในการสลับแต่ละครั้ง
    public int shuffleTimes = 10; // จำนวนครั้งในการสลับ

    void Start()
    {
        StartCoroutine(LiftCupAndStartGame()); // เริ่มการยกถ้วยและเริ่มเกม
    }

    IEnumerator LiftCupAndStartGame()
    {
        ballPosition = Random.Range(0, cups.Length);

        cups[ballPosition].transform.position += new Vector3(0, liftHeight, 0);
        yield return new WaitForSeconds(2);
        cups[ballPosition].transform.position -= new Vector3(0, liftHeight, 0);

        ball.transform.SetParent(cups[ballPosition].transform);
        ball.transform.localPosition = new Vector3(0, 0.5f, 0);

        yield return StartCoroutine(WaitForPlayerClick());
        StartCoroutine(ShuffleAnimation());
    }

    IEnumerator WaitForPlayerClick()
    {
        Debug.Log("คลิกเพื่อเริ่มการสลับถ้วย...");
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
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

            if (ballPosition == cupA)
            {
                ballPosition = cupB;
                ball.transform.SetParent(cups[cupB].transform);
            }
            else if (ballPosition == cupB)
            {
                ballPosition = cupA;
                ball.transform.SetParent(cups[cupA].transform);
            }

            ball.transform.localPosition = new Vector3(0, 0.5f, 0);
            yield return new WaitForSeconds(0.1f);
        }
        shuffling = false;
        gameStarted = true;
        Debug.Log("การสลับเสร็จสิ้น คลิกที่ถ้วยเพื่อเลือก");
    }

    public void CheckCup(int selectedIndex)
    {
        if (gameStarted)
        {
            // ยกถ้วยที่ผู้เล่นเลือกขึ้น
            StartCoroutine(LiftSelectedCup(selectedIndex));

            if (selectedIndex == ballPosition)
            {
                Debug.Log("คุณทายถูก! ลูกบอลอยู่ใต้ถ้วยนี้");
            }
            else
            {
                Debug.Log("คุณทายผิด! ลูกบอลอยู่ใต้ถ้วยที่ " + (ballPosition + 1));
                StartCoroutine(RevealAllCups());
            }

            gameStarted = false;
        }
    }

    IEnumerator LiftSelectedCup(int selectedIndex)
    {
        // ยกถ้วยที่ถูกเลือกขึ้น
        cups[selectedIndex].transform.position += new Vector3(0, 2, 0);
        yield return new WaitForSeconds(1); // รอให้ผู้เล่นเห็นการยกถ้วย
    }

    IEnumerator RevealAllCups()
    {
        for (int i = 0; i < cups.Length; i++)
        {
            if (i != ballPosition)
            {
                cups[i].transform.position += new Vector3(0, 2, 0);
            }
        }
        yield return null;
    }

    public bool IsShuffling()
    {
        return shuffling;
    }
}
