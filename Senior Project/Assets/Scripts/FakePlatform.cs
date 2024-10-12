using System.Collections;
using UnityEngine;

public class FakePlatform : MonoBehaviour
{
    public float disappearDelay = 0f; // ให้หายไปทันทีที่ผู้เล่นชน

    private bool hasBeenUsed = false; // ตรวจสอบว่าแพลตฟอร์มถูกใช้หรือยัง
    private Collider platformCollider;

    private void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformCollider.isTrigger = true; // ทำให้แพลตฟอร์มทะลุผ่านได้จากด้านล่าง
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenUsed && other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // ตรวจสอบว่าผู้เล่นชนจากด้านบนของแพลตฟอร์ม
            if (rb != null && other.transform.position.y > transform.position.y)
            {
                hasBeenUsed = true; // ตรวจสอบว่าแพลตฟอร์มถูกใช้แล้ว

                // แสดงข้อความเพื่อยืนยันว่าชนกับแพลตฟอร์มหลอกจากด้านบน
                Debug.Log("Player hit the fake platform from the top, it will disappear.");

                // ทำให้แพลตฟอร์มหายไปทันที
                StartCoroutine(Disappear());
            }
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay); // รอเวลาที่กำหนดก่อนแพลตฟอร์มหายไป (ตอนนี้คือทันที)
        Destroy(gameObject); // ลบวัตถุแพลตฟอร์มออกจากเกม
    }
}