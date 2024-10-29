using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.3f;
    public Vector3 offset;
    public float cameraThreshold = 0.2f;
    public GameObject gameOverUI;

    private float highestYPosition;
    private bool gameIsOver = false;
    private bool isFallingToEnd = false;
    private ScoremanagerScene2 scoreManager;

    private void Start()
    {
        highestYPosition = transform.position.y;
        gameOverUI.SetActive(false);

        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }

        // เรียกใช้การทำลายแพลตฟอร์มเป็นระยะเพื่อลดการใช้งานทรัพยากรในแต่ละเฟรม
        InvokeRepeating("DetectAndDestroyPlatforms", 0.5f, 0.5f);
    }

    private void LateUpdate()
    {
        if (gameIsOver || isFallingToEnd) return;

        if (target.position.y > highestYPosition + cameraThreshold)
        {
            highestYPosition = target.position.y - cameraThreshold;
            Vector3 desiredPosition = new Vector3(transform.position.x, highestYPosition + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        // ตรวจสอบว่าตัวละครตกลงมาต่ำกว่ากล้อง
        if (target.position.y < transform.position.y - 0.6f && !gameIsOver)
        {
            Debug.Log("Player fell below camera threshold - initiating end game check.");
            isFallingToEnd = true;
            gameIsOver = true;  // ตั้งค่านี้เป็น true เพื่อป้องกันการเรียกซ้ำ
            scoreManager?.EndGameWithJigsawCheck();
        }
    }

    private void DetectAndDestroyPlatforms()
    {
        // ค้นหาแพลตฟอร์มที่อยู่นอกขอบหน้าจอและทำลายเพื่อเพิ่มประสิทธิภาพ
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("TrampolinePlatform");

        foreach (GameObject platform in platforms)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(platform.transform.position);

            if (viewportPos.y < 0)
            {
                Destroy(platform);
            }
        }
    }
}