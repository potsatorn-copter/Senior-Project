using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Itempool : MonoBehaviour
{
    public GameObject goodItemPrefab;
    public GameObject badItemPrefab;
    public static int poolSize = 10;  // จำกัดไอเท็มให้มีเพียง 10 ชิ้น
    public TextMeshProUGUI resultText; // อ้างอิงไปยัง TextMeshProUGUI สำหรับแสดงผลลัพธ์

    private static List<GameObject> itemsPool;
    private int itemsActivated = 0; // ตัวแปรสำหรับนับจำนวนไอเท็มที่ถูกเรียกใช้
    private bool poolCompleted = false;

    public ScoreManager1 scoreManagerPlayer;
    public ScoreAI scoreManagerAI;
    public GameObject winPanel;
    public GameObject losePanel;


    private void Awake()
    {
        // ซ่อน resultText ที่เริ่มเกม
            if (resultText != null)
                resultText.gameObject.SetActive(false);
            if(winPanel != null)
                winPanel.gameObject.SetActive(false);
            if(losePanel != null)
                losePanel.gameObject.SetActive(false);
        
        
            // สร้าง list ของไอเท็ม
        itemsPool = new List<GameObject>(poolSize);
        
        // เพิ่มไอเท็มตามลำดับที่ต้องการ (ปรับลำดับที่นี่)
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = (i % 2 == 0) ? goodItemPrefab : badItemPrefab;
            var newItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newItem.SetActive(false);
            itemsPool.Add(newItem);
        }
    }

    public GameObject GetItemFromPool()
    {
        if (poolCompleted) return null; // ถ้าวนครบแล้ว จะไม่วนไอเท็มอีก

        if (itemsPool.Count > 0)
        {
            var item = itemsPool[0];
            item.SetActive(true);
            itemsPool.RemoveAt(0);
            itemsActivated++;

            // ตรวจสอบหากไอเท็มถูกเรียกใช้ครบ 10 ครั้ง
            if (itemsActivated >= poolSize)
            {
                poolCompleted = true; // กำหนดว่าวนครบแล้ว
                SummarizeScores();
            }

            return item;
        }
        else
        {
            Debug.Log("No items left in the pool!");
            return null;
        }
    }

    public static void ReturnItemToPool(GameObject item)
    {
        item.SetActive(false);
        if (itemsPool.Count < poolSize)
        {
            itemsPool.Add(item);  // เพิ่มไอเท็มกลับเข้า list ที่ตำแหน่งท้ายสุด
        }
        else
        {
            Debug.Log("Item pool is full!");
            // อาจจะทำลายไอเท็มหรือจัดการอย่างอื่น
        }
    }
    private void SummarizeScores()
    {
        // การเริ่ม Coroutine เพื่อรอ 3 วินาทีก่อนแสดงผลลัพธ์
        StartCoroutine(SummarizeScoresCoroutine());
    }
    
    private IEnumerator SummarizeScoresCoroutine()
    {
        int playerScore = scoreManagerPlayer.GetCurrentScore();
        int aiScore = scoreManagerAI.GetCurrentScoreAI();
        
        yield return new WaitForSeconds(3);

        Debug.Log("Final Scores:");
        Debug.Log("Player Score: " + playerScore);
        Debug.Log("AI Score: " + aiScore);

        if (playerScore > aiScore)
        {
            // ถ้าคะแนนเพลเยอร์มากกว่า AI
            winPanel.gameObject.SetActive(true);
            resultText.text = "WIN";
            resultText.color = Color.green; // ตั้งค่าสีเป็นเขียว
        }
        else if (playerScore < aiScore)
        {
            // ถ้าคะแนน AI มากกว่าเพลเยอร์
            losePanel.gameObject.SetActive(true);
            resultText.text = "LOSE";
            resultText.color = Color.red; // ตั้งค่าสีเป็นแดง
        }
        else
        {
            // ถ้าคะแนนเท่ากัน
            resultText.text = "DRAW";
            resultText.color = Color.yellow; // ตั้งค่าสีเป็นเหลือง
        }

        // อาจจะแสดงผลลัพธ์บน UI หรือจบเกม
    }
}
