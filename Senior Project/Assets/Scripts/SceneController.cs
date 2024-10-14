using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2; // 3 แถว
    public const int gridCols = 6; // 4 คอลัมน์
    public const float offsetX = 3f;
    public const float offsetY = 5f;
    private int successfulMatches = 0;
    public const int totalMatches = gridRows * gridCols / 2; // 6 คู่
    private float timeRemaining = 30f; // เวลา
    private bool isGameOver = false; // สถานะเกมจบหรือไม่

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI timerText; // อ้างอิง Text UI สำหรับแสดงเวลา
    [SerializeField] private TextMeshProUGUI matchesLabel; // อ้างอิงถึง TextMeshPro สำหรับแสดงจำนวนคู่ที่จับได้

    private void Start()
    {
        Vector3 startPos = originalCard.transform.position; // ตำแหน่งของการ์ดแรก

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 , 4, 4, 5, 5};
        numbers = ShuffleArray(numbers); // ฟังก์ชันสุ่มลำดับการ์ด

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MainCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MainCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.ChangeSprite(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }

        gameOverUI.SetActive(false); // ซ่อน UI เมื่อเริ่มเกม
    }
    
    private void Update()
    {
        if (isGameOver)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // ลดเวลาที่เหลือลงทุกเฟรม
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString() + "s"; // อัปเดตเวลาที่เหลือบน UI
        }
        else
        {
            timeRemaining = 0; // ให้แน่ใจว่าเวลาไม่ต่ำกว่า 0
            GameOver(); // เรียกฟังก์ชัน GameOver เมื่อเวลาหมด
            isGameOver = true; // ป้องกันไม่ให้ GameOver ถูกเรียกซ้ำ
        }
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    private MainCard _firstRevealed;
    private MainCard _secondRevealed;

    private int _score = 0;

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    public void CardRevealed(MainCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            successfulMatches++;
        
            // อัปเดตการแสดงผลของจำนวนคู่ที่จับได้
            matchesLabel.text = "Matches: " + successfulMatches + "/6";

            SoundManager.instance.Play(SoundManager.SoundName.Correct);

            if (successfulMatches == totalMatches)
            {
                GameOver();
            }
        }
        else
        {
            SoundManager.instance.Play(SoundManager.SoundName.Wrong);
            yield return new WaitForSeconds(0.5f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }

        _firstRevealed = null;
        _secondRevealed = null;
    }

    private void GameOver()
    {
        SoundManager.instance.Play(SoundManager.SoundName.WinSound);
    
        Debug.Log("GameOver function called"); // ตรวจสอบว่าฟังก์ชันนี้ถูกเรียกจริงหรือไม่
    
        // ปรับการคิดคะแนนตามจำนวนคู่ที่จับได้
        if (successfulMatches >= 5)
        {
            _score = 10; // จับคู่ได้ 5-6 คู่
        }
        else if (successfulMatches >= 3)
        {
            _score = 6; // จับคู่ได้ 3-4 คู่
        }
        else if (successfulMatches >= 1)
        {
            _score = 2; // จับคู่ได้ 1-2 คู่
        }
        else
        {
            _score = 0; // ไม่มีคู่ที่จับได้เลย
        }
    
        scoreLabel.text = "Final Score: " + _score; // แสดงคะแนนสุดท้าย
        gameOverUI.SetActive(true); // แสดง UI เมื่อเกมจบ
    }

}