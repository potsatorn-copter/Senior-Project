using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatpawController : MonoBehaviour
{
    public Transform targetPosition;
    private Vector3 startPosition;
    public ScoreManager1 scoreManager;
    public float delayBeforeReturning = 0.2f;

    private void Start()
    {
        // บันทึกตำแหน่งเริ่มต้น
        startPosition = transform.position;
    }

    public void OnButtonClick()
    {
        MoveToTargetPosition();
    }

    private void MoveToTargetPosition()
    {
        // ย้ายไปยังตำแหน่งเป้าหมาย
        transform.position = targetPosition.position;
        StartCoroutine(ReturnToStartPositionAfterDelay());
    }

    private IEnumerator ReturnToStartPositionAfterDelay()
    {
        // รอเวลาที่ตั้งไว้
        yield return new WaitForSeconds(delayBeforeReturning);
        // กลับสู่ตำแหน่งเริ่มต้น
        transform.position = startPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoodItem"))
        {
            // เป็น GoodItem บวกคะแนน 1
            Debug.Log("This is Good Player");
            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
                Debug.Log("+1 Score");
            }
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
            // เป็น BadItem ลบคะแนน 1
            Debug.Log("This is Bad Player");
            if (scoreManager != null)
            {
                scoreManager.SubtractScore(1);
                Debug.Log("-1 Score");
            }
        }
    
        // ตรวจสอบว่าได้รับ Item script จาก GameObject ที่ชน
        Item itemScript = other.GetComponent<Item>();
        if (itemScript != null)
        {
            // คืนไอเท็มกลับสู่ pool
            itemScript.Deactivate();
            Itempool.ReturnItemToPool(other.gameObject); // ตรวจสอบให้แน่ใจว่า itemPool ถูกอ้างอิงถูกต้อง
        }
    }
}
