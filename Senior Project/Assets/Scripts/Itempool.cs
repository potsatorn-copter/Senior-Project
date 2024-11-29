using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // สำหรับการใช้งาน Text UI

public class Itempool : MonoBehaviour
{
    public GameObject goodItemPrefab;
    public GameObject badItemPrefab;

    public Sprite[] goodItemSpritesEasy;   // Sprite for Easy (2 sprites)
    public Sprite[] goodItemSpritesNormal; // Sprite for Normal (3 sprites)
    public Sprite[] goodItemSpritesHard;   // Sprite for Hard (3 sprites)

    public Sprite[] badItemSpritesEasy;    // Sprite for Easy (1 sprite)
    public Sprite[] badItemSpritesNormal;  // Sprite for Normal (2 sprites)
    public Sprite[] badItemSpritesHard;    // Sprite for Hard (2 sprites)

    public TextMeshProUGUI poolCounterText; // Text UI สำหรับแสดงจำนวนไอเท็มใน Pool

    public static int poolSize;  // Dynamic pool size based on difficulty level

    private static List<GameObject> goodItemsPool;
    private static List<GameObject> badItemsPool;
    private int itemsActivated = 0;
    private bool poolCompleted = false;

    private Sprite[] currentGoodItemSprites;
    private Sprite[] currentBadItemSprites;
    private int[] spawnPattern;
    private int patternIndex = 0;

    private void Awake()
    {
        // Check difficulty level from GameSettings
        if (GameSettings.difficultyLevel == 0)  // Easy Mode
        {
            // Easy Mode (12 good items, 6 bad items)
            SetupPool(12, 6, goodItemSpritesEasy, badItemSpritesEasy);
            spawnPattern = new int[] { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1 };
        }
        else if (GameSettings.difficultyLevel == 1)  // Normal Mode
        {
            // Normal Mode (15 good items, 7 bad items)
            SetupPool(15, 7, goodItemSpritesNormal, badItemSpritesNormal);
            spawnPattern = new int[] { 1, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1 };
        }
        else if (GameSettings.difficultyLevel == 2)  // Hard Mode
        {
            // Hard Mode (16 good items, 8 bad items)
            SetupPool(16, 8, goodItemSpritesHard, badItemSpritesHard);
            spawnPattern = new int[] { 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0 };
        }

        UpdatePoolCounterUI(); // อัปเดตจำนวนไอเท็มที่เหลือใน Pool ตอนเริ่มต้น
    }

    // Setup the pool based on the number of good and bad items
    private void SetupPool(int goodItemCount, int badItemCount, Sprite[] goodItemSprites, Sprite[] badItemSprites)
    {
        poolSize = goodItemCount + badItemCount;
        currentGoodItemSprites = goodItemSprites;
        currentBadItemSprites = badItemSprites;

        goodItemsPool = new List<GameObject>(goodItemCount);
        badItemsPool = new List<GameObject>(badItemCount);

        // Add good items to the pool
        for (int i = 0; i < goodItemCount; i++)
        {
            var goodItem = Instantiate(goodItemPrefab, Vector3.zero, Quaternion.identity);
            goodItem.SetActive(false);
            goodItemsPool.Add(goodItem);
        }

        // Add bad items to the pool
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

        GameObject itemToSpawn = null;

        // Check the next item type in the pattern
        if (spawnPattern[patternIndex] == 1 && goodItemsPool.Count > 0)
        {
            itemToSpawn = goodItemsPool[0];
            goodItemsPool.RemoveAt(0);

            // Set the sprite for the good item
            SpriteRenderer spriteRenderer = itemToSpawn.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && currentGoodItemSprites.Length > 0)
            {
                spriteRenderer.sprite = currentGoodItemSprites[Random.Range(0, currentGoodItemSprites.Length)];
            }
        }
        else if (spawnPattern[patternIndex] == 0 && badItemsPool.Count > 0)
        {
            itemToSpawn = badItemsPool[0];
            badItemsPool.RemoveAt(0);

            // Set the sprite for the bad item
            SpriteRenderer spriteRenderer = itemToSpawn.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && currentBadItemSprites.Length > 0)
            {
                spriteRenderer.sprite = currentBadItemSprites[Random.Range(0, currentBadItemSprites.Length)];
            }
        }

        // Move to the next pattern index, loop back if needed
        patternIndex = (patternIndex + 1) % spawnPattern.Length;

        // If no items left in the respective pool, check if all items have been activated
        if (itemToSpawn != null)
        {
            itemToSpawn.SetActive(true);
            itemsActivated++;
            UpdatePoolCounterUI(); // อัปเดตจำนวนไอเท็มที่เหลือใน Pool

            if (itemsActivated >= poolSize)
            {
                poolCompleted = true;
            }
        }

        return itemToSpawn;
    }

    private void UpdatePoolCounterUI()
    {
        if (poolCounterText != null)
        {
            int remainingItems = poolSize - itemsActivated;
            poolCounterText.text = $"ไอเทมทั้งหมด: {remainingItems}";
        }
    }

    // Mark the item as inactive instead of returning it to the pool
    public static void MarkItemAsInactive(GameObject item)
    {
        item.SetActive(false);
    }

    public bool IsPoolCompleted()
    {
        return poolCompleted;
    }
}
