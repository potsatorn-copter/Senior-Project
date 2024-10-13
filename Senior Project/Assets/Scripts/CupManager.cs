using UnityEngine;
using System.Collections;

public class CupManager : MonoBehaviour
{
    public GameObject[] cups; // ถ้วยทั้งหมด
    public GameObject ball; // ลูกบอล
    private Transform cupWithBall; // ถ้วยที่มีลูกบอลอยู่
    private bool shuffling = false; // สถานะการสลับถ้วย
    private bool gameStarted = false; // สถานะเริ่มเกม
    public float liftHeight = 2.0f; // ความสูงที่ถ้วยยกขึ้น
    public float shuffleDuration = 1.0f; // ระยะเวลาที่ใช้ในการสลับแต่ละครั้ง
    public int shuffleTimes = 10; // จำนวนครั้งในการสลับ

    void Start()
    {
        StartCoroutine(ShowBallThenCover()); // เริ่มด้วยการแสดงลูกบอล
    }

    // แสดงลูกบอลที่อยู่ใต้ถ้วยในรอบแรก แล้วเอาถ้วยลงมาปิด
    IEnumerator ShowBallThenCover()
    {
        int initialBallPosition = Random.Range(0, cups.Length); // เลือกถ้วยที่จะเริ่มมีลูกบอล
        cupWithBall = cups[initialBallPosition].transform; // บันทึกถ้วยที่มีลูกบอลอยู่

        // ยกถ้วยขึ้นเพื่อแสดงลูกบอล
        cupWithBall.position += new Vector3(0, liftHeight, 0);

        // กำหนดตำแหน่งของลูกบอลให้อยู่ใต้ถ้วยที่ถูกยก พร้อมกับปรับให้ลูกบอลต่ำลงอีกนิด
        ball.transform.SetParent(cupWithBall);
        ball.transform.localPosition = new Vector3(0, -3.0f, 0); // ปรับตำแหน่งลูกบอลให้อยู่ต่ำลงกว่าเดิม

        yield return new WaitForSeconds(2); // รอให้ผู้เล่นเห็นลูกบอล

        // วางถ้วยลงเพื่อปิดลูกบอล
        cupWithBall.position -= new Vector3(0, liftHeight, 0);

        // รีเซ็ตตำแหน่งลูกบอลหลังจากถ้วยวางลง
        ball.transform.localPosition = new Vector3(0, -0.5f, 0); // ปรับตำแหน่งลูกบอลให้อยู่ใต้ถ้วยหลังวางลง

        yield return StartCoroutine(WaitForPlayerClick());
        StartCoroutine(ShuffleAnimation()); // เริ่มสลับถ้วยหลังจากปิดลูกบอล
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
        Debug.Log("Ball before shuffle");

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
                // สลับตำแหน่งของถ้วย A และ B โดยใช้ Lerp
                cups[cupA].transform.position = Vector3.Lerp(cupAPosition, cupBPosition, (elapsedTime / shuffleDuration));
                cups[cupB].transform.position = Vector3.Lerp(cupBPosition, cupAPosition, (elapsedTime / shuffleDuration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ทำให้แน่ใจว่าถ้วยสลับตำแหน่งกันเสร็จสิ้น
            cups[cupA].transform.position = cupBPosition;
            cups[cupB].transform.position = cupAPosition;

            // ตรวจสอบว่าถ้วยที่มีลูกบอลคือถ้วยไหน และทำให้ลูกบอลติดตามถ้วยนั้นเสมอ
            if (cupWithBall == cups[cupA].transform)
            {
                cupWithBall = cups[cupB].transform; // อัปเดตว่าถ้วยที่มีลูกบอลได้ถูกย้ายไปที่ถ้วย B
            }
            else if (cupWithBall == cups[cupB].transform)
            {
                cupWithBall = cups[cupA].transform; // อัปเดตว่าถ้วยที่มีลูกบอลได้ถูกย้ายไปที่ถ้วย A
            }

            Debug.Log($"Ball after shuffle {i + 1}: Ball is now at the correct cup");

            yield return new WaitForSeconds(0.1f);
        }

        // เมื่อการสลับเสร็จสิ้น ให้ย้ายลูกบอลกลับไปที่ถ้วยที่ถูกต้อง
        ball.transform.SetParent(cupWithBall);
        ball.transform.localPosition = new Vector3(0, -0.5f, 0); // ปรับตำแหน่งลูกบอลให้อยู่ใต้ถ้วยเสมอ

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

            if (cups[selectedIndex].transform == cupWithBall)
            {
                Debug.Log("คุณทายถูก! ลูกบอลอยู่ใต้ถ้วยนี้");

                // แสดงลูกบอลเมื่อทายถูก
                ball.transform.SetParent(null); // ปลดบอลออกจากการเป็นลูกของถ้วย
                ball.transform.position = cups[selectedIndex].transform.position + new Vector3(0, -3.0f, 0); // วางลูกบอลในตำแหน่งเดิมเหมือนตอนเปิดถ้วยครั้งแรก
                ball.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                Debug.Log("คุณทายผิด! ยกถ้วยที่เลือกและถ้วยที่ถูกต้อง");
                StartCoroutine(RevealCorrectAndSelectedCup(selectedIndex));
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

    // ยกถ้วยที่เลือกและถ้วยที่มีลูกบอล
    IEnumerator RevealCorrectAndSelectedCup(int selectedIndex)
    {
        // ยกถ้วยที่ผู้เล่นเลือกขึ้น
        cups[selectedIndex].transform.position += new Vector3(0, 2, 0);
        
        // รอให้ผู้เล่นเห็นถ้วยที่เลือกผิดถูกยกขึ้น
        yield return new WaitForSeconds(1);

        // ยกถ้วยที่มีลูกบอลขึ้นและแสดงบอล
        cupWithBall.position += new Vector3(0, 2, 0);
        ball.transform.SetParent(null); // ปลดบอลออกจากการเป็นลูกของถ้วย
        ball.transform.position = cupWithBall.position + new Vector3(0, -3.0f, 0); // แสดงลูกบอลในระดับ -3.0f
        ball.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(1); // รอให้ผู้เล่นเห็นการยกถ้วยที่ถูกต้อง
    }

    public bool IsShuffling()
    {
        return shuffling;
    }
}