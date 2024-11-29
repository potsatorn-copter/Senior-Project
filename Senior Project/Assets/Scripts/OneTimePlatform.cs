using System.Collections;
using UnityEngine;

public class OneTimePlatform : MonoBehaviour
{
    public float bounceForce = 3f; // แรงดีดกลับเมื่อผู้เล่นเหยียบ
    public float disappearDelay = 1.0f; // กำหนดเวลาหลังจากที่ผู้เล่นเหยียบแล้วแพลตฟอร์มจะหายไป
    private bool hasBeenUsed = false; // ตรวจสอบว่าแพลตฟอร์มถูกใช้หรือยัง
    public bool isSteppedOn = false; // ตรวจสอบว่าแพลตฟอร์มถูกเหยียบหรือยัง
    private Collider2D platformCollider;
    private ScoremanagerScene2 scoreManager; // ตัวแปรเก็บอ้างอิงถึง ScoremanagerScene2

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        platformCollider.isTrigger = true; // ทำให้แพลตฟอร์มทะลุผ่านได้จากด้านล่าง

        // ค้นหา ScoremanagerScene2 ในซีนปัจจุบัน
        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenUsed && other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
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

                // เพิ่มคะแนนเมื่อเหยียบแพลตฟอร์ม หาก scoreManager มีอยู่
                if (scoreManager != null)
                {
                    scoreManager.AddScore(100);
                }

                // เพิ่มแรงกระโดด
                rb.velocity = new Vector2(rb.velocity.x, 0f); // รีเซ็ตความเร็วในแนวดิ่ง
                rb.AddForce(Vector2.up * finalBounceForce, ForceMode2D.Impulse); // ส่งแรงขึ้นด้านบน
                playerController.StartSmoothFall();
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
