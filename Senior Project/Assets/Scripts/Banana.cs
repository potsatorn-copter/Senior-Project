using UnityEngine;

public class Banana : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // ตรวจสอบว่าเป็นการชนกับผู้เล่นหรือไม่
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collected the banana!"); // แสดงข้อความเมื่อชนกับผู้เล่น

            // เรียกฟังก์ชันเก็บไอเทมกล้วยในผู้เล่น
            GrandpaController playerController = collision.gameObject.GetComponent<GrandpaController>();
            if (playerController != null)
            {
                playerController.CollectBanana(); // เปิดการใช้งานบูสต์การกระโดด
                Debug.Log("Boost activated!"); // แสดงข้อความเมื่อเปิดใช้งานบูสต์
            }
            
            SoundManager.instance.Play(SoundManager.SoundName.Eat);

            // ลบกล้วยออกจากเกม
            gameObject.SetActive(false);
        }
    }
}