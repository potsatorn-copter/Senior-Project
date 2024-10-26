using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICatPaw : MonoBehaviour
{
    public Transform targetPosition;
    private Vector3 startPosition;
    public float delayBeforeReturning = 0.2f;
    public float movementInterval = 5.0f; // กำหนดความถี่ในการเคลื่อนไหว
    private float nextMoveTime = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // ตรวจสอบว่าถึงเวลาที่ควรเคลื่อนไหวหรือไม่
        if (Time.time >= nextMoveTime)
        {
            StartCoroutine(WaitAndMove(4.0f)); // เรียกใช้ Coroutine ด้วยเวลาหน่วง 4 วินาที
            // กำหนดเวลาสำหรับการเคลื่อนไหวครั้งถัดไป
            nextMoveTime = Time.time + movementInterval + delayBeforeReturning;
        }
    }
    private IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // รอ 4 วินาที
        MoveToTargetPosition(); // จากนั้นเรียกใช้ MoveToTargetPosition
    }

    private void MoveToTargetPosition()
    {
        transform.position = targetPosition.position;
        StartCoroutine(ReturnToStartPositionAfterDelay());
    }

    private IEnumerator ReturnToStartPositionAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeReturning);
        transform.position = startPosition;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoodItem"))
        {
           
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
           
        }
    
        // ตรวจสอบว่าได้รับ Item script จาก GameObject ที่ชน
        Item itemScript = other.GetComponent<Item>();
        if (itemScript != null)
        {
            // คืนไอเท็มกลับสู่ pool
            itemScript.Deactivate();
        }
    }
}

