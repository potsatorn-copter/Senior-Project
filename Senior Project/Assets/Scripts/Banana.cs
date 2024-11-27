using UnityEngine;

public class Banana : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าเป็นการชนกับผู้เล่นหรือไม่
        if (collision.CompareTag("Player"))
        {
            // เรียกฟังก์ชันเก็บไอเทมกล้วยในผู้เล่น
            GrandpaController playerController = collision.GetComponent<GrandpaController>();
            if (playerController != null)
            {
                playerController.CollectBanana(); // เปิดการใช้งานบูสต์การกระโดด
            }

            // เล่นเสียงเมื่อเก็บกล้วย
            SoundManager.instance.Play(SoundManager.SoundName.Eat);

            // ลบกล้วยออกจากเกม
            Destroy(gameObject);
        }
    }
}