using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemspawner : MonoBehaviour
{
    public Itempool itemPool;
    public Transform spawnPoint;
    public ScoreManager1 scoreManager; // อ้างอิงไปยัง ScoreManager เพื่อจบเกมเมื่อไอเท็มหมด

    private float spawnDelay; // ดีเลย์สำหรับการเกิดของไอเท็ม

    private void Start()
    {
        // กำหนดดีเลย์ตามระดับความยาก
        if (GameSettings.difficultyLevel == 0) // Easy
        {
            spawnDelay = 4f; // โหมดง่าย ดีเลย์ 6 วินาที
        }
        else if (GameSettings.difficultyLevel == 1) // Normal
        {
            spawnDelay = 3f; // โหมดกลาง ดีเลย์ 4 วินาที
        }
        else if (GameSettings.difficultyLevel == 2) // Hard
        {
            spawnDelay = 2f; // โหมดยาก ดีเลย์ 3 วินาที
        }

        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay); // ใช้ดีเลย์ที่ตั้งไว้ตามระดับความยาก

            var itemGameObject = itemPool.GetItemFromPool();
            Item item = null;

            if (itemGameObject != null)
            {
                item = itemGameObject.GetComponent<Item>();
                if (item != null && spawnPoint != null)
                {
                    item.Activate(spawnPoint.position);
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

            // Deactivate item after delay
            if (item != null)
            {
                yield return new WaitForSeconds(3f); // หน่วงเวลาก่อนปิดการใช้งานไอเท็ม
                item.Deactivate();
            }
        }
    }
}