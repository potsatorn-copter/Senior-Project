using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itempool : MonoBehaviour
{
    public GameObject goodItemPrefab;
    public GameObject badItemPrefab;

    public Sprite[] goodItemSpritesEasy;   // Sprite สำหรับ Easy (2 รูป)
    public Sprite[] goodItemSpritesNormal; // Sprite สำหรับ Normal (3 รูป)
    public Sprite[] goodItemSpritesHard;   // Sprite สำหรับ Hard (3 รูป)

    public Sprite[] badItemSpritesEasy;    // Sprite สำหรับ Easy (1 รูป)
    public Sprite[] badItemSpritesNormal;  // Sprite สำหรับ Normal (2 รูป)
    public Sprite[] badItemSpritesHard;    // Sprite สำหรับ Hard (2 รูป)

    public static int poolSize;  // ขนาดของ pool ไดนามิกตามโหมด

    private static List<GameObject> goodItemsPool;
    private static List<GameObject> badItemsPool;
    private int itemsActivated = 0;
    private bool poolCompleted = false;

    private Sprite[] currentGoodItemSprites;  // Sprite ที่ถูกใช้ในโหมดปัจจุบัน
    private Sprite[] currentBadItemSprites;   // Sprite สำหรับไอเทมเสียในโหมดปัจจุบัน

    private void Awake()
    {
        // ตรวจสอบระดับความยากที่เก็บใน GameSettings
        if (GameSettings.difficultyLevel == 0)  // Easy Mode
        {
            // Easy Mode (2 ไอเทมดี, 1 ไอเทมไม่ดี)
            SetupPool(12, 6, goodItemSpritesEasy, badItemSpritesEasy);
        }
        else if (GameSettings.difficultyLevel == 1)  // Normal Mode
        {
            // Normal Mode (3 ไอเทมดี, 2 ไอเทมไม่ดี)
            SetupPool(15, 7, goodItemSpritesNormal, badItemSpritesNormal);
        }
        else if (GameSettings.difficultyLevel == 2)  // Hard Mode
        {
            // Hard Mode (3 ไอเทมดี, 2 ไอเทมไม่ดี)
            SetupPool(16, 8, goodItemSpritesHard, badItemSpritesHard);
        }
    }

    // ฟังก์ชันตั้งค่า pool
    private void SetupPool(int goodItemCount, int badItemCount, Sprite[] goodItemSprites, Sprite[] badItemSprites)
    {
        // กำหนดค่าให้ poolSize และ Sprite
        poolSize = goodItemCount + badItemCount;
        currentGoodItemSprites = goodItemSprites;
        currentBadItemSprites = badItemSprites;

        goodItemsPool = new List<GameObject>(goodItemCount);
        badItemsPool = new List<GameObject>(badItemCount);

        // สร้างไอเท็มดีเข้า pool
        for (int i = 0; i < goodItemCount; i++)
        {
            var goodItem = Instantiate(goodItemPrefab, Vector3.zero, Quaternion.identity);
            goodItem.SetActive(false);
            goodItemsPool.Add(goodItem);
        }

        // สร้างไอเท็มเสียเข้า pool
        for (int i = 0; i < badItemCount; i++)
        {
            var badItem = Instantiate(badItemPrefab, Vector3.zero, Quaternion.identity);
            badItem.SetActive(false);
            badItemsPool.Add(badItem);
        }
    }

    public GameObject GetItemFromPool()
    {
        if (poolCompleted) return null;

        float phase = (float)itemsActivated / poolSize;
        float goodItemChance = 1.0f;

        // ปรับอัตราส่วนการสุ่มไอเท็มตามเฟสของเกม
        if (phase < 0.33f)
        {
            goodItemChance = 0.65f;  // ช่วงแรก 65% เป็นไอเท็มดี
        }
        else if (phase < 0.66f)
        {
            goodItemChance = 0.55f;  // ช่วงกลาง 55% เป็นไอเท็มดี
        }
        else
        {
            goodItemChance = 0.50f;  // ช่วงท้าย 50/50 ระหว่างดีและไม่ดี
        }

        GameObject itemToSpawn;
        if (Random.value <= goodItemChance && goodItemsPool.Count > 0)
        {
            itemToSpawn = goodItemsPool[0];
            goodItemsPool.RemoveAt(0); // เอาไอเท็มออกจาก pool อย่างถาวร

            // สุ่มเลือก Sprite ให้กับไอเท็มดี
            SpriteRenderer spriteRenderer = itemToSpawn.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && currentGoodItemSprites.Length > 0)
            {
                spriteRenderer.sprite = currentGoodItemSprites[Random.Range(0, currentGoodItemSprites.Length)];
            }
        }
        else if (badItemsPool.Count > 0)
        {
            itemToSpawn = badItemsPool[0];
            badItemsPool.RemoveAt(0); // เอาไอเท็มออกจาก pool อย่างถาวร

            // สุ่มเลือก Sprite ให้กับไอเท็มเสีย
            SpriteRenderer spriteRenderer = itemToSpawn.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && currentBadItemSprites.Length > 0)
            {
                spriteRenderer.sprite = currentBadItemSprites[Random.Range(0, currentBadItemSprites.Length)];
            }
        }
        else
        {
            return null;  // หากไม่มีไอเท็มเหลือ
        }

        itemToSpawn.SetActive(true);
        itemsActivated++;

        // เช็คว่าไอเท็มใน pool ถูกดึงออกครบแล้วหรือยัง
        if (itemsActivated >= poolSize)
        {
            poolCompleted = true;
        }

        return itemToSpawn;
    }

    public static void ReturnItemToPool(GameObject item)
    {
        item.SetActive(false);

        // เอาไอเท็มกลับเข้า pool ตามประเภทได้ หากต้องการในภายหลัง
        if (item.CompareTag("GoodItem") && goodItemsPool.Count < 12)
        {
            goodItemsPool.Add(item);
        }
        else if (item.CompareTag("BadItem") && badItemsPool.Count < 6)
        {
            badItemsPool.Add(item);
        }
    }

    // ฟังก์ชันเช็คว่า Pool หมดแล้วหรือยัง
    public bool IsPoolCompleted()
    {
        return poolCompleted;
    }
}