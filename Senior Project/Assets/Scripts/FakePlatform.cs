using System.Collections;
using UnityEngine;

public class FakePlatform : MonoBehaviour
{
    public float disappearDelay = 0f; // ให้หายไปทันทีที่ผู้เล่นชน

    private bool hasBeenUsed = false; // ตรวจสอบว่าแพลตฟอร์มถูกใช้หรือยัง
    public bool isSteppedOn = false; // ตรวจสอบว่าแพลตฟอร์มถูกเหยียบหรือยัง
    private Collider platformCollider;

    private void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformCollider.isTrigger = true; // ทำให้แพลตฟอร์มทะลุผ่านได้จากด้านล่าง
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSteppedOn && other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // ตรวจสอบว่าผู้เล่นชนจากด้านบนของแพลตฟอร์ม
            if (rb != null && other.transform.position.y > transform.position.y)
            {
                isSteppedOn = true; // บันทึกว่าผู้เล่นเหยียบแพลตฟอร์มแล้ว

                // แสดงข้อความเพื่อยืนยันว่าชนกับแพลตฟอร์มหลอกจากด้านบน
                Debug.Log("Player hit the fake platform from the top, it will disappear.");

                // ทำให้แพลตฟอร์มหายไปทันที
                StartCoroutine(Disappear());
            }
        }

        if (!hasBeenUsed && other.CompareTag("Player"))
        {
            // โค้ดที่ใช้กับ hasBeenUsed เดิมยังคงอยู่
            hasBeenUsed = true;
            // อาจมีการเรียกใช้ฟังก์ชันหรือทำงานเพิ่มเติมที่เกี่ยวข้องกับการใช้แพลตฟอร์ม
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay); // รอเวลาที่กำหนดก่อนแพลตฟอร์มหายไป
        Destroy(gameObject); // ลบวัตถุแพลตฟอร์มออกจากเกม
    }
}