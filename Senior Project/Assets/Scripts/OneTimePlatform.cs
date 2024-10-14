using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimePlatform : MonoBehaviour
{
     public float bounceForce = 3f; // แรงดีดกลับเมื่อผู้เล่นเหยียบ
    public float disappearDelay = 0.5f; // กำหนดเวลาหลังจากที่ผู้เล่นเหยียบแล้วแพลตฟอร์มจะหายไป
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

                // เพิ่มคะแนนเมื่อเหยียบแพลตฟอร์ม
                ScoremanagerScene2.Instance.AddScore(100);

                // เพิ่มแรงกระโดด
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // รีเซ็ตความเร็วในแนวดิ่ง
                rb.AddForce(Vector3.up * finalBounceForce, ForceMode.Impulse);  // ส่งแรงขึ้นด้านบน
                SoundManager.instance.Play(SoundManager.SoundName.Jump);

                // ทำให้แพลตฟอร์มใช้ได้ครั้งเดียวแล้วหายไป
                hasBeenUsed = true;
                StartCoroutine(Disappear()); // ทำให้แพลตฟอร์มหายไปหลังจากใช้งาน
            }
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay); // รอเวลาที่กำหนดก่อนแพลตฟอร์มหายไป
        Destroy(gameObject); // ลบวัตถุแพลตฟอร์มออกจากเกม
    }
}

