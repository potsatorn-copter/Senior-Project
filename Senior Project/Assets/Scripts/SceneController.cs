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
    public JigsawUIPanel jigsawUIPanel;

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
            timeRemaining = 30f;
            totalMatches = gridRows * gridCols / 2; // 6 คู่
        }
        else if (GameSettings.difficultyLevel == 1) // Normal
        {
            gridRows = 3;
            gridCols = 6;
            offsetY = 3f;
            timeRemaining = 60f;
            totalMatches = gridRows * gridCols / 2; // 9 คู่
        }
        else if (GameSettings.difficultyLevel == 2) // Hard
        {
            gridRows = 3;
            gridCols = 6;
            offsetY = 3f;
            timeRemaining = 50f;
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
            timerText.text = "เหลือเวลา: " + Mathf.Ceil(timeRemaining).ToString() + "s";
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
            matchesLabel.text = "สำเร็จ: " + successfulMatches + "/" + totalMatches;
            SoundManager.instance.Play(SoundManager.SoundName.CorrectItem);

            if (successfulMatches == totalMatches)
            {
                GameOver();
            }
        }
        else
        {
            SoundManager.instance.Play(SoundManager.SoundName.WrongItem);
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

        int finalScore = 0;

        if (GameSettings.difficultyLevel == 0) // Easy
        {
            if (successfulMatches >= 5)
                finalScore = 10; 
            else if (successfulMatches >= 3)
                finalScore = 6; 
            else if (successfulMatches >= 1)
                finalScore = 2; 
            else
                finalScore = 0; 
        }
        else if (GameSettings.difficultyLevel == 1) // Normal
        {
            if (successfulMatches >= 7)
                finalScore = 10;
            else if (successfulMatches >= 5)
                finalScore = 6;
            else if (successfulMatches >= 3)
                finalScore = 4;
            else if (successfulMatches >= 1)
                finalScore = 2;
            else
                finalScore = 0;
        }
        else if (GameSettings.difficultyLevel == 2) // Hard
        {
            if (successfulMatches >= 7)
                finalScore = 10;
            else if (successfulMatches >= 5)
                finalScore = 6;
            else if (successfulMatches >= 3)
                finalScore = 4;
            else if (successfulMatches >= 1)
                finalScore = 2;
            else
                finalScore = 0;
        }

        scoreLabel.text = "คะแนนที่ได้ : " + finalScore;
        isGameOver = true;

        ScoreManager.Instance.SetScoreForScene(3, finalScore);
        Debug.Log("Score for Scene 3 set in ScoreManager: " + finalScore);

        DropJigsawPieceIfEligible(finalScore);
    }

    private void DropJigsawPieceIfEligible(int finalScore)
    {
        if (finalScore >= 8)
        {
            int imageIndex = GameSettings.difficultyLevel;
            JigsawManager jigsawManager = FindObjectOfType<JigsawManager>();

            if (jigsawManager != null)
            {
                JigsawManager.JigsawImage jigsawImage = jigsawManager.jigsawImages[imageIndex];
                JigsawManager.JigsawPiece droppedPiece = jigsawImage.jigsawPieces[2]; // ชิ้นส่วนที่ index 2

                // ตรวจสอบว่าชิ้นส่วนนั้นถูกเก็บไปแล้วหรือไม่
                if (!droppedPiece.isCollected)
                {
                    // เก็บชิ้นส่วนถ้ายังไม่ถูกเก็บ
                    jigsawManager.CollectJigsawPiece(imageIndex, 2);
                    Debug.Log($"Jigsaw piece dropped: Scene 3, Difficulty Level: {GameSettings.difficultyLevel}, Image Part 3: {imageIndex}, Final Score: {finalScore}");

                    if (jigsawUIPanel != null)
                    {
                        Sprite jigsawSprite = droppedPiece.pieceSprite;
                        jigsawUIPanel.ShowJigsawUIPanel(jigsawSprite, "คุณได้รับชิ้นส่วนจิ๊กซอว์ใหม่!");
                        StartCoroutine(ShowEndGamePanelWithDelay());
                    }
                    else
                    {
                        Debug.LogWarning("JigsawUIPanel is not found in the scene.");
                        ShowEndGamePanel();
                    }
                }
                else
                {
                    // แสดงข้อความแจ้งเตือนว่าชิ้นส่วนนี้ถูกเก็บไปแล้ว
                    Debug.Log("Jigsaw piece already collected - no UI shown.");
                    ShowEndGamePanel();
                }
            }
            else
            {
                Debug.LogWarning("JigsawManager is not found in the scene.");
                ShowEndGamePanel();
            }
        }
        else
        {
            Debug.Log("No jigsaw piece dropped in Scene 3. Final score below threshold.");
            ShowEndGamePanel();
        }
    }

    private IEnumerator ShowEndGamePanelWithDelay()
    {
        yield return new WaitForSeconds(3f);
        if (jigsawUIPanel != null)
        {
            jigsawUIPanel.HideJigsawUIPanel();
        }
        yield return new WaitForSeconds(0.5f);
        ShowEndGamePanel();
    }

    private void ShowEndGamePanel()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}