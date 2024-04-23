using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAI : MonoBehaviour
{
    public ScoreAI scoreManager;
    public float delayBeforeReturning = 0.1f; 
    public float movementSpeed = 1.0f; 
    public LayerMask itemLayerMask;
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 110f;

    private Vector3 startPosition;
    private float nextMoveTime = 0f;
    private Transform closestItemTransform = null;
    private bool isMoving = false; // ตัวแปรใหม่เพื่อตรวจสอบว่า AI กำลังเคลื่อนที่หรือไม่

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Time.time >= nextMoveTime && !isMoving) // เพิ่มการตรวจสอบ isMoving
        {
            FindClosestItem();
            if (closestItemTransform != null)
            {
                StartCoroutine(MoveToTargetPosition(closestItemTransform.position));
            }
        }
    }

    void FindClosestItem()
    {
        Collider[] itemsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, itemLayerMask);
        float closestDistance = Mathf.Infinity;
        Collider closestItem = null;

        foreach (Collider item in itemsInViewRadius)
        {
            Vector3 dirToItem = (item.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToItem) < viewAngle / 2)
            {
                float distanceToItem = Vector3.Distance(transform.position, item.transform.position);
                if (distanceToItem < closestDistance && item.gameObject.activeInHierarchy)
                {
                    closestDistance = distanceToItem;
                    closestItem = item;
                }
            }
        }

        closestItemTransform = closestItem != null ? closestItem.transform : null;
    }

    IEnumerator MoveToTargetPosition(Vector3 targetPos)
    {
        isMoving = true; // ตั้งค่า isMoving เป็น true เมื่อเริ่มเคลื่อนที่

        yield return new WaitForSeconds(2f); // Delay 2 วินาทีก่อนเริ่มเคลื่อนที่

        float elapsedTime = 0;
        Vector3 originalPosition = transform.position;
        while (elapsedTime < movementSpeed)
        {
            if (closestItemTransform == null || !closestItemTransform.gameObject.activeInHierarchy)
            {
                break; // หยุดการเคลื่อนที่หากเป้าหมายไม่อยู่แล้ว
            }
            
            transform.position = Vector3.Lerp(originalPosition, targetPos, elapsedTime / movementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition; // กลับสู่ตำแหน่งเริ่มต้น
        isMoving = false; // ตั้งค่า isMoving เป็น false เมื่อการเคลื่อนที่เสร็จสิ้น
        closestItemTransform = null; // รีเซ็ตเป้าหมายเมื่อการเคลื่อนที่เสร็จสิ้น
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoodItem"))
        {
            // เป็น GoodItem บวกคะแนน 1
            Debug.Log("This is Good AI Smart");
            if (scoreManager != null)
            {
                scoreManager.AddScoreAI(1);
                Debug.Log("+1 Score");
            }
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
            // เป็น BadItem ลบคะแนน 1
            Debug.Log("This is Bad Smart");
            if (scoreManager != null)
            {
                scoreManager.SubtractScoreAI(1);
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

