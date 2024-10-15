using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private int gridRows;
    private int gridCols;
    private int totalMatches;
    private float timeRemaining;
    private int successfulMatches = 0;
    private bool isGameOver = false;
    private float offsetX = 3f;
    private float offsetY;
    public float previewTime = 6.0f; // เวลาที่จะแสดงการ์ดก่อนปิด

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI matchesLabel;

    private MainCard[] cards; // เก็บการ์ดทั้งหมดในเกม
    private Coroutine previewCoroutine;

    private void Start()
    {
        SetupGameDifficulty();
        SetupCards();

        gameOverUI.SetActive(false); // ซ่อน UI เมื่อเริ่มเกม

        // เริ่มการแสดงการ์ด
        previewCoroutine = StartCoroutine(PreviewCardsAndStartGame());
    }

    // ฟังก์ชันสำหรับการแสดงการ์ดและเริ่มเกม
    private IEnumerator PreviewCardsAndStartGame()
    {
        // แสดงการ์ดทุกใบ
        foreach (MainCard card in cards)
        {
            card.Reveal();
        }

        // รอเวลาตามที่กำหนด (เช่น 3 วินาที)
        yield return new WaitForSeconds(previewTime);

        // ปิดการ์ดทุกใบหลังจากช่วงเวลาที่กำหนด
        foreach (MainCard card in cards)
        {
            card.Unreveal();
        }

        // เริ่มนับเวลาเกมหลังจากแสดงการ์ดเสร็จ
        StartCoroutine(GameTimer());
    }

    // ฟังก์ชันสำหรับการตั้งค่าความยากของเกม
    private void SetupGameDifficulty()
    {
        if (GameSettings.difficultyLevel == 0) // Easy
        {
            gridRows = 2;
            gridCols = 6;
            offsetY = 4f;
            timeRemaining = 50f;
            totalMatches = gridRows * gridCols / 2; // 6 คู่
        }
        else if (GameSettings.difficultyLevel == 1) // Normal
        {
            gridRows = 3;
            gridCols = 6;
            offsetY = 3f;
            timeRemaining = 80f;
            totalMatches = gridRows * gridCols / 2; // 9 คู่
        }
        else if (GameSettings.difficultyLevel == 2) // Hard
        {
            gridRows = 3;
            gridCols = 6;
            offsetY = 3f;
            timeRemaining = 70f;
            totalMatches = gridRows * gridCols / 2; // 9 คู่
        }
    }

    // ฟังก์ชันสำหรับการตั้งค่าการ์ดในเกม
    private void SetupCards()
    {
        Vector3 startPos = originalCard.transform.position;
        int[] numbers = new int[totalMatches * 2];
        for (int i = 0; i < totalMatches; i++)
        {
            numbers[2 * i] = i;
            numbers[2 * i + 1] = i;
        }

        numbers = ShuffleArray(numbers);
        cards = new MainCard[gridCols * gridRows]; // เก็บการ์ดทั้งหมด

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

                cards[index] = card; // เก็บการ์ดในอาร์เรย์
            }
        }
    }

    private void Update()
    {
        if (isGameOver)
            return;
    }
    public void SetEasyMode()  // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Easy
    {
        GameSettings.difficultyLevel = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // รีโหลด Scene เพื่อทดสอบ
    }

    public void SetNormalMode()  // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Normal
    {
        GameSettings.difficultyLevel = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // รีโหลด Scene เพื่อทดสอบ
    }

    public void SetHardMode()  // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Hard
    {
        GameSettings.difficultyLevel = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // รีโหลด Scene เพื่อทดสอบ
    }

    // ฟังก์ชันเปลี่ยนระดับความยากและรีโหลดฉาก
    private void ChangeDifficulty(int level)
    {
        GameSettings.difficultyLevel = level;

        // หยุดการแสดงการ์ดชั่วคราว
        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
        }

        // รีโหลดฉาก
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ฟังก์ชันนับถอยหลังเวลา
    private IEnumerator GameTimer()
    {
        while (timeRemaining > 0 && !isGameOver)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString() + "s";
            yield return null;
        }

        if (timeRemaining <= 0)
        {
            GameOver();
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
            matchesLabel.text = "Matches: " + successfulMatches + "/" + totalMatches;
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

        int _score = 0;

        if (GameSettings.difficultyLevel == 0) // Easy
        {
            if (successfulMatches >= 5)
                _score = 10; 
            else if (successfulMatches >= 3)
                _score = 6; 
            else if (successfulMatches >= 1)
                _score = 2; 
            else
                _score = 0; 
        }
        else if (GameSettings.difficultyLevel == 1 || GameSettings.difficultyLevel == 2) // Normal & Hard
        {
            if (successfulMatches >= 7)
                _score = 10;
            else if (successfulMatches >= 5)
                _score = 6;
            else if (successfulMatches >= 3)
                _score = 4;
            else if (successfulMatches >= 1)
                _score = 2;
            else
                _score = 0;
        }

        scoreLabel.text = "Final Score: " + _score;
        gameOverUI.SetActive(true);
        isGameOver = true;
    }
}