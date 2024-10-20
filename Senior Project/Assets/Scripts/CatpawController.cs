using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatpawController : MonoBehaviour
{
    public Transform itemSpawnerTransform; // Reference to Itemspawner's Transform
    private Vector3 startPosition;
    public ScoreManager1 scoreManager;
    public float delayBeforeReturning = 0.2f;

    private bool canClick = true; // ตัวแปรควบคุมการกดปุ่ม

    private void Start()
    {
        // บันทึกตำแหน่งเริ่มต้น
        startPosition = transform.position;
    }

    public void OnButtonClick()
    {
        if (canClick) // ตรวจสอบว่ากดปุ่มได้หรือไม่
        {
            canClick = false; // ปิดการกดปุ่มระหว่างรอการเคลื่อนที่และการคืนตำแหน่ง
            MoveToTargetPosition();
        }
    }

    private void MoveToTargetPosition()
    {
        // ย้ายไปยังตำแหน่งของ Itemspawner
        if (itemSpawnerTransform != null)
        {
            transform.position = itemSpawnerTransform.position; 
        }
        else
        {
            Debug.LogWarning("Itemspawner Transform is not assigned!");
        }
        StartCoroutine(ReturnToStartPositionAfterDelay());
    }

    private IEnumerator ReturnToStartPositionAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeReturning);
        transform.position = startPosition; // กลับสู่ตำแหน่งเริ่มต้น
        canClick = true; // เปิดให้กดปุ่มได้อีกครั้ง
    }

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าไอเท็มที่ชนเป็น GoodItem หรือ BadItem
        if (other.gameObject.CompareTag("GoodItem"))
        {
            Debug.Log("Good Item Collected");
            SoundManager.instance.Play(SoundManager.SoundName.CorrectItem);

            // เมื่อชนไอเท็มดี เพิ่มตัวนับ
            if (scoreManager != null)
            {
                StartCoroutine(DelayCollectGoodItem()); // รอให้มือชนแล้วค่อยเพิ่มตัวนับ
            }
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
            Debug.Log("Bad Item Collected");
            SoundManager.instance.Play(SoundManager.SoundName.WrongItem);

            // เมื่อชนไอเท็มไม่ดี ลดตัวนับ
            if (scoreManager != null)
            {
                StartCoroutine(DelayCollectBadItem()); // รอให้มือชนแล้วค่อยลดตัวนับ
            }
        }

        Item itemScript = other.GetComponent<Item>();
        if (itemScript != null)
        {
            itemScript.Deactivate();
            Itempool.ReturnItemToPool(other.gameObject);
        }
    }

    // เพิ่มฟังก์ชัน Coroutine สำหรับหน่วงเวลาก่อนเพิ่มตัวนับ
    private IEnumerator DelayCollectGoodItem()
    {
        yield return new WaitForSeconds(0.1f); // หน่วงเวลาเล็กน้อย
        scoreManager.CollectGoodItem(); // เพิ่มจำนวนไอเท็มดีที่เก็บได้หลังจากชนแล้ว
        Debug.Log("+1 Good Item Count");
    }

    // เพิ่มฟังก์ชัน Coroutine สำหรับหน่วงเวลาก่อนลดตัวนับ
    private IEnumerator DelayCollectBadItem()
    {
        yield return new WaitForSeconds(0.1f); // หน่วงเวลาเล็กน้อย
        scoreManager.CollectBadItem(); // ลดจำนวนไอเท็มเมื่อเก็บไอเท็มไม่ดีหลังจากชนแล้ว
        Debug.Log("-1 Good Item Count");
    }
}