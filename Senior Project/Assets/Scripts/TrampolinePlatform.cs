using System.Collections;
using UnityEngine;

public class TrampolinePlatform : MonoBehaviour
{
    public float bounceForce = 3f;
    public bool isOneTime = false;  // ตัวแปรระบุว่าเป็นแพลตฟอร์มชนิดใช้ครั้งเดียวหรือไม่
    public float disappearDelay = 0.5f; // สำหรับแพลตฟอร์ม OneTime กำหนดเวลาหลังจากที่ผู้เล่นชนแล้วจะหายไป

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
            GrandpaController playerController = other.GetComponent<GrandpaController>();

            // ตรวจสอบว่าผู้เล่นอยู่สูงกว่าแพลตฟอร์มก่อนชน
            if (rb != null && playerController != null && other.transform.position.y > transform.position.y)
            {
                // ตรวจสอบว่าผู้เล่นมีบูสต์จากกล้วยหรือไม่
                float finalBounceForce = bounceForce;
                if (playerController.hasJumpBoost)
                {
                    finalBounceForce *= playerController.boostMultiplier; // เพิ่มแรงกระโดดเมื่อมีบูสต์
                    playerController.hasJumpBoost = false; // ใช้บูสต์แล้ว
                }

                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // รีเซ็ตความเร็วในแนวดิ่ง
                rb.AddForce(Vector3.up * finalBounceForce, ForceMode.Impulse);  // ส่งแรงขึ้นด้านบน
                SoundManager.instance.Play(SoundManager.SoundName.Jump);

                if (isOneTime) // ถ้าเป็นแพลตฟอร์มชนิดใช้ครั้งเดียว
                {
                    hasBeenUsed = true; // ทำให้แพลตฟอร์มถูกใช้แล้ว
                    StartCoroutine(Disappear()); // ทำให้แพลตฟอร์มหายไปหลังจากใช้งาน
                }
            }
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay); // รอเวลาที่กำหนดก่อนแพลตฟอร์มหายไป
        Destroy(gameObject); // ลบวัตถุแพลตฟอร์มออกจากเกม
    }
}