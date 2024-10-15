using System.Collections;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public float moveDistance = 3.0f; // ระยะที่มอนสเตอร์จะขยับ
    public float moveSpeed = 2.0f; // ความเร็วในการขยับ
    public int damageAmount = 50; // จำนวนคะแนนที่จะถูกลบ (-50 หรือ -100)
    public bool startMovingLeft = true; // ให้เลือกว่าจะเริ่มขยับไปทางซ้ายหรือขวา
    public float waitTimeAtEdge = 1.0f; // เวลาที่จะหยุดก่อนกลับทิศทาง
    public float closeEnoughDistance = 0.01f; // ระยะที่พิจารณาว่าถึงจุดแล้ว (เพื่อความ Smooth)

    public float gizmoLineLength = 1.0f; // ความยาวของเส้น Gizmo ที่จะปรับได้ใน Inspector

    private Vector3 startPosition;
    private bool movingTowardsDestination; // ตัวแปรบอกว่ากำลังไปยังจุดปลายทางหรือไม่
    private Vector3 targetPosition; // ตำแหน่งปลายทาง

    private void Start()
    {
        startPosition = transform.position;

        // กำหนดตำแหน่งปลายทางตามการตั้งค่าเริ่มต้น
        if (startMovingLeft)
        {
            targetPosition = startPosition - new Vector3(moveDistance, 0, 0); // ไปทางซ้าย
        }
        else
        {
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0); // ไปทางขวา
        }

        movingTowardsDestination = true; // เริ่มขยับไปยังปลายทาง
    }

    private void Update()
    {
        MoveMonster();
    }

    // ฟังก์ชันสำหรับขยับมอนสเตอร์
    void MoveMonster()
    {
        if (movingTowardsDestination)
        {
            // ขยับไปยังปลายทาง
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // เมื่อมาถึงปลายทาง ให้กลับไปที่จุดเริ่มต้น
            if (Vector3.Distance(transform.position, targetPosition) <= closeEnoughDistance) // ใช้ค่า closeEnoughDistance เพื่อให้ smooth ขึ้น
            {
                StartCoroutine(WaitAndReturnToStart());
            }
        }
    }

    // ฟังก์ชันสำหรับการกลับไปจุดเริ่มต้น พร้อมรอที่ขอบปลายทาง
    IEnumerator WaitAndReturnToStart()
    {
        movingTowardsDestination = false; // หยุดขยับไปยังปลายทาง

        // รอเวลาสักครู่ที่ปลายทาง
        yield return new WaitForSeconds(waitTimeAtEdge);

        // กลับไปที่จุดเริ่มต้น
        while (Vector3.Distance(transform.position, startPosition) > closeEnoughDistance) // ใช้ค่า closeEnoughDistance
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = startPosition; // กลับถึงจุดเริ่มต้น

        // รอเวลาสักครู่ที่จุดเริ่มต้นก่อนเดินไปยังจุดปลายทางอีกครั้ง
        yield return new WaitForSeconds(waitTimeAtEdge);

        movingTowardsDestination = true;
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อมีการชนกับ Player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ลดคะแนนเมื่อชนกับ Player
            ScoremanagerScene2.Instance.AddScore(-damageAmount);
            Debug.Log("Player hit monster! Score deducted by: " + damageAmount);
        }
    }

    void OnDrawGizmos()
    {
        // แสดงเส้น Gizmo ตามทิศทางการเดินซ้ายหรือขวา
        Gizmos.color = Color.yellow;

        if (startMovingLeft)
        {
            // ถ้าเริ่มจากการเดินซ้าย ให้เส้น Gizmo ชี้ไปทางซ้ายด้วยความยาวที่กำหนด
            Gizmos.DrawLine(transform.position, transform.position - new Vector3(gizmoLineLength, 0, 0));
        }
        else
        {
            // ถ้าเริ่มจากการเดินขวา ให้เส้น Gizmo ชี้ไปทางขวาด้วยความยาวที่กำหนด
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(gizmoLineLength, 0, 0));
        }
    }
}