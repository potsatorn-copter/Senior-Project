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
    private ScoremanagerScene2 scoreManager; // ตัวแปรเก็บอ้างอิงถึง ScoremanagerScene2

    private void Start()
    {
        highestYPosition = transform.position.y; // เริ่มต้นที่ตำแหน่งปัจจุบันของกล้อง
        gameOverUI.SetActive(false); // ซ่อน UI เมื่อเริ่มเกม

        // ค้นหา ScoremanagerScene2
        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }
    }

    private void LateUpdate()
    {
        if (gameIsOver) return;

        if (target.position.y > highestYPosition + cameraThreshold)
        {
            highestYPosition = target.position.y - cameraThreshold;

            Vector3 desiredPosition = new Vector3(transform.position.x, highestYPosition + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        if (target.position.y < transform.position.y - 0.7f)
        {
            GameOver();
        }

        DetectAndDestroyPlatforms();
    }

    private void GameOver()
    {
        gameIsOver = true;
        if (scoreManager != null) scoreManager.EndGame();
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        SoundManager.instance.Play(SoundManager.SoundName.LoseSound);
    }

    private void DetectAndDestroyPlatforms()
    {
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