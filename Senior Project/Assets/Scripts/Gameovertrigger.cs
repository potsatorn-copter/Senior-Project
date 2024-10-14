using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverUI; // Drag and drop your Game Over UI panel here

    private void OnCollisionEnter(Collision collision)
    {
        // ตรวจสอบว่าตัวที่ชนมี tag เป็น Player หรือไม่
        if (collision.gameObject.CompareTag("Player"))
        {
            // หยุดเกม
            Time.timeScale = 0f;

            // แสดง UI
            if (gameOverUI != null)
            {
                SoundManager.instance.Play(SoundManager.SoundName.LoseSound);
                gameOverUI.SetActive(true);
            }
        }
    }
}