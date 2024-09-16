using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ตัวละครที่กล้องจะติดตาม
    public float smoothSpeed = 0.1f; // ความเร็วในการเลื่อนกล้อง
    public Vector3 offset; // การชดเชยตำแหน่งของกล้อง (ค่าเริ่มต้น)
    public float cameraThreshold = 0.2f; // ระยะที่ตัวละครต้องขึ้นไปถึงก่อนที่กล้องจะเลื่อนตาม
    public GameObject gameOverUI; // UI ที่จะโชว์เมื่อเกมจบ

    private float highestYPosition; // ตำแหน่ง Y สูงสุดที่กล้องเคยตาม
    private bool gameIsOver = false;

    private void Start()
    {
        highestYPosition = transform.position.y; // เริ่มต้นที่ตำแหน่งปัจจุบันของกล้อง
        gameOverUI.SetActive(false); // ซ่อน UI เมื่อเริ่มเกม
    }

    private void LateUpdate()
    {
        if (gameIsOver) return;

        // ตรวจสอบว่าตัวละครอยู่สูงกว่าตำแหน่งที่กล้องเคยตามหรือไม่
        if (target.position.y > highestYPosition + cameraThreshold)
        {
            highestYPosition = target.position.y - cameraThreshold; // ปรับตำแหน่ง Y สูงสุดใหม่

            // คำนวณตำแหน่งที่ต้องการให้กล้องเลื่อนไป
            Vector3 desiredPosition = new Vector3(transform.position.x, highestYPosition + offset.y, transform.position.z);

            // ใช้ Lerp เพื่อทำให้การเลื่อนกล้องราบรื่น
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // เลื่อนกล้องไปยังตำแหน่งที่คำนวณได้
            transform.position = smoothedPosition;
        }

        // ตรวจสอบว่าตัวละครตกต่ำกว่ากล้องหรือไม่
        if (target.position.y < transform.position.y - 0.7)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameIsOver = true;
        Time.timeScale = 0f; // หยุดเกมโดยการหยุดเวลา
        SoundManager.instance.Play(SoundManager.SoundName.LoseSound);
        gameOverUI.SetActive(true); // แสดง UI
    }
}