using System.Collections;
using System.Collections.Generic;
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
        if (target.position.y < transform.position.y - 0.7f)
        {
            GameOver();
        }

        // ตรวจจับแพลตฟอร์มที่พ้นกล้อง
        DetectAndDestroyPlatforms();
    }

    private void GameOver()
    {
        gameIsOver = true;
        ScoremanagerScene2.Instance.EndGame(); 
        gameOverUI.SetActive(true); // แสดง UI
        Time.timeScale = 0f; // หยุดเกมโดยการหยุดเวลา
        SoundManager.instance.Play(SoundManager.SoundName.LoseSound);
    }

    // ฟังก์ชันสำหรับตรวจจับแพลตฟอร์มที่พ้นกล้องและทำลายมัน
    private void DetectAndDestroyPlatforms()
    {
        // ค้นหาแพลตฟอร์มทั้งหมดในเกม
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("TrampolinePlatform");

        // ตรวจสอบว่าแพลตฟอร์มใดพ้นกล้องไปแล้ว
        foreach (GameObject platform in platforms)
        {
            // แปลงตำแหน่งของแพลตฟอร์มจาก world space ไปเป็น viewport space
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(platform.transform.position);

            // ถ้าแพลตฟอร์มอยู่นอกขอบล่างของกล้อง (viewport Y < 0) แสดงว่ามันพ้นจากการมองเห็นแล้ว
            if (viewportPos.y < 0)
            {
                Destroy(platform); // ทำลายแพลตฟอร์มที่พ้นกล้องไปแล้ว
            }
        }
    }
}