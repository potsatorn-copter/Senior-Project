using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Itemspawner : MonoBehaviour
{
    public Itempool itemPool;
    public Transform spawnPoint;
    public ScoreManager1 scoreManager;

    public GameObject startGameUI; // UI ที่จะแสดงปุ่มเริ่มเกม
    public TextMeshProUGUI countdownText;    // ข้อความสำหรับแสดงเลขถอยหลัง

    private float spawnDelay; // ดีเลย์สำหรับการเกิดของไอเท็ม
    private bool isGameStarted = false; // สถานะเริ่มเกม

    private void Start()
    {
        // กำหนดดีเลย์ตามระดับความยาก
        if (GameSettings.difficultyLevel == 0) // Easy
        {
            spawnDelay = 2f;
        }
        else if (GameSettings.difficultyLevel == 1) // Normal
        {
            spawnDelay = 1f;
        }
        else if (GameSettings.difficultyLevel == 2) // Hard
        {
            spawnDelay = 0.5f;
        }

        // แสดง UI เริ่มเกม
        startGameUI.SetActive(true);

        // ซ่อนข้อความนับถอยหลังเริ่มต้น
        countdownText.gameObject.SetActive(false);
    }

    // ฟังก์ชันที่เรียกเมื่อกดปุ่มเริ่มเกม
    public void OnStartGameButtonPressed()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            startGameUI.SetActive(false); // ปิด UI เริ่มเกม
            StartCoroutine(StartCountdownAndSpawn());
        }
    }

    // แสดงเลขถอยหลัง 3 2 1 ก่อนเริ่มเกม
    private IEnumerator StartCountdownAndSpawn()
    {
        // เปิดข้อความนับถอยหลัง
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); // แสดงเลขถอยหลัง
            yield return new WaitForSeconds(1f); // รอ 1 วินาที
        }

        // ลบข้อความถอยหลังและซ่อน Text
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);

        StartCoroutine(SpawnItems()); // เริ่ม Spawn ไอเท็ม
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            // เรียกไอเท็มจาก Pool
            var itemGameObject = itemPool.GetItemFromPool();
            Item item = null;

            if (itemGameObject != null)
            {
                item = itemGameObject.GetComponent<Item>();
                if (item != null && spawnPoint != null)
                {
                    item.Activate(spawnPoint.position); // เปิดใช้งานไอเท็มที่ตำแหน่ง Spawn
                }
                else
                {
                    Debug.LogError("Item or spawnPoint is null.");
                }
            }

            // เช็คว่าไอเท็มใน Pool หมดหรือไม่ ถ้าหมดให้จบเกม
            if (itemPool.IsPoolCompleted())
            {
                yield return new WaitForSeconds(2.0f); // ดีเลย์ 2 วินาทีก่อนจบเกม
                scoreManager.CalculateFinalScore(); // เรียกใช้ฟังก์ชันคำนวณคะแนน
                break; // ออกจากลูปเมื่อไอเท็มหมด
            }

            // รอเวลา 3 วินาทีเพื่อให้ไอเท็มค้างอยู่ในซีนก่อน Deactivate
            if (item != null)
            {
                yield return new WaitForSeconds(3f);
                item.Deactivate(); // ปิดการใช้งานไอเท็ม
            }

            // หลังจาก Deactivate ไอเท็ม ให้รอ spawnDelay ก่อนสร้างไอเท็มใหม่
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
