using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;


public class JigsawManager : MonoBehaviour
{
    public static JigsawManager Instance;

    [System.Serializable]
    public class JigsawPiece
    {
        public string pieceName;
        public Sprite pieceSprite;
        public bool isCollected = false;
    }

    [System.Serializable]
    public class JigsawImage
    {
        public string imageName;
        public Sprite completedImage;
        public Sprite lockedImage;
        public List<JigsawPiece> jigsawPieces;

        public bool IsComplete()
        {
            foreach (var piece in jigsawPieces)
            {
                if (!piece.isCollected)
                    return false;
            }

            return true;
        }
    }

    public List<JigsawImage> jigsawImages;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var jigsawImage in jigsawImages)
        {
            LoadJigsawProgress(jigsawImage);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsGalleryScene())
        {
            UpdateGallery();
        }
    }

    private void UpdateSingleGalleryImage(Image galleryImage, JigsawImage jigsawImage, string unlockKey)
    {
        if (galleryImage != null)
        {
            // ตรวจสอบสถานะการปลดล็อคจาก PlayerPrefs
            bool isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;

            // หากปลดล็อคแล้วให้แสดงภาพที่สมบูรณ์ทันที
            if (isUnlocked)
            {
                galleryImage.sprite = jigsawImage.completedImage;
            }
            else
            {
                // หากยังไม่ปลดล็อคแสดงภาพที่ถูกล็อค
                galleryImage.sprite = jigsawImage.lockedImage;

                // เพิ่ม Listener สำหรับการกดเพื่อปลดล็อค
                Button galleryButton = galleryImage.GetComponent<Button>();
                if (galleryButton != null)
                {
                    galleryButton.onClick.RemoveAllListeners(); // ลบ Listener เดิม
                    galleryButton.onClick.AddListener(() =>
                    {
                        if (jigsawImage.IsComplete())
                        {
                            // เล่นอนิเมชันปลดล็อค
                            Animator animator = galleryImage.GetComponent<Animator>();
                            if (animator != null)
                            {
                                animator.SetTrigger("Unlock");
                            }

                            // บันทึกสถานะการปลดล็อคใน PlayerPrefs
                            PlayerPrefs.SetInt(unlockKey, 1);
                            PlayerPrefs.Save();

                            // แสดงภาพที่สมบูรณ์
                            galleryImage.sprite = jigsawImage.completedImage;

                            Debug.Log($"{jigsawImage.imageName} has been unlocked!");
                        }
                        else
                        {
                            Debug.Log("Jigsaw image is not complete yet.");
                        }
                    });
                }
            }
        }
    }


    public void UpdateGallery()
    {
        if (IsGalleryScene())
        {
            GameObject easyImageObj = GameObject.Find("EasyImage");
            GameObject normalImageObj = GameObject.Find("NormalImage");
            GameObject hardImageObj = GameObject.Find("HardImage");

            if (easyImageObj && normalImageObj && hardImageObj)
            {
                UpdateSingleGalleryImage(easyImageObj.GetComponent<Image>(), jigsawImages[0], "isEasyUnlocked");
                UpdateSingleGalleryImage(normalImageObj.GetComponent<Image>(), jigsawImages[1], "isNormalUnlocked");
                UpdateSingleGalleryImage(hardImageObj.GetComponent<Image>(), jigsawImages[2], "isHardUnlocked");
            }
            else
            {
                Debug.LogWarning("One or more gallery images are missing in the Gallery Scene.");
            }
        }
    }

    public void CollectJigsawPiece(int difficultyLevel, int sceneIndex)
    {
        if (difficultyLevel < jigsawImages.Count)
        {
            JigsawImage currentJigsaw = jigsawImages[difficultyLevel];
            if (sceneIndex < currentJigsaw.jigsawPieces.Count)
            {
                JigsawPiece piece = currentJigsaw.jigsawPieces[sceneIndex];

                if (!piece.isCollected)
                {
                    piece.isCollected = true;
                    Debug.Log($"Collected piece: {piece.pieceName} for {currentJigsaw.imageName}");
                    SaveJigsawProgress(currentJigsaw);

                    if (currentJigsaw.IsComplete())
                    {
                        Debug.Log($"{currentJigsaw.imageName} is now complete!");

                        // บันทึกสถานะว่า "จิ๊กซอว์ครบ" (แต่ยังไม่ปลดล็อค)
                        if (currentJigsaw.imageName == "ImageEasy")
                        {
                            PlayerPrefs.SetInt("isEasyCompleted", 1);
                            Debug.Log("isEasyCompleted set to 1");
                        }
                        else if (currentJigsaw.imageName == "ImageNormal")
                        {
                            PlayerPrefs.SetInt("isNormalCompleted", 1);
                            Debug.Log("isNormalCompleted set to 1");
                        }
                        else if (currentJigsaw.imageName == "ImageHard")
                        {
                            PlayerPrefs.SetInt("isHardCompleted", 1);
                            Debug.Log("isHardCompleted set to 1");
                        }

                        PlayerPrefs.Save();
                    }
                }
                else
                {
                    Debug.LogWarning($"Piece {piece.pieceName} for {currentJigsaw.imageName} is already collected.");
                }
            }
            else
            {
                Debug.LogWarning("Invalid sceneIndex for jigsawPieces.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid difficultyLevel for jigsawImages.");
        }
    }

    private void SaveJigsawProgress(JigsawImage jigsawImage)
    {
        foreach (var piece in jigsawImage.jigsawPieces)
        {
            // บันทึกเฉพาะชิ้นส่วนที่ถูกเก็บ
            if (piece.isCollected)
            {
                PlayerPrefs.SetInt(jigsawImage.imageName + "_" + piece.pieceName, 1);
            }
        }

        PlayerPrefs.Save();
    }

    private void LoadJigsawProgress(JigsawImage jigsawImage)
    {
        foreach (var piece in jigsawImage.jigsawPieces)
        {
            // โหลดสถานะของแต่ละชิ้นส่วนแยกกัน
            piece.isCollected = PlayerPrefs.GetInt(jigsawImage.imageName + "_" + piece.pieceName, 0) == 1;
        }
    }

    private bool IsGalleryScene()
    {
        return SceneManager.GetActiveScene().name == "MemoryGallery";
    }

    public void ResetJigsawProgress()
    {
        // รีเซ็ตสถานะของชิ้นส่วนจิ๊กซอว์และลบข้อมูลการเก็บชิ้นส่วน
        foreach (var jigsawImage in jigsawImages)
        {
            foreach (var piece in jigsawImage.jigsawPieces)
            {
                piece.isCollected = false;
                PlayerPrefs.DeleteKey(jigsawImage.imageName + "_" + piece.pieceName);
            }
        }

        // รีเซ็ตสถานะการแจ้งเตือนทั้งหมด
        PlayerPrefs.SetInt("hasShownEasyUnlockNotification", 0);
        PlayerPrefs.SetInt("hasShownNormalUnlockNotification", 0);
        PlayerPrefs.SetInt("hasShownHardUnlockNotification", 0);

        // รีเซ็ตสถานะความสำเร็จและการปลดล็อคใน PlayerPrefs
        PlayerPrefs.SetInt("isEasyCompleted", 0);
        PlayerPrefs.SetInt("isNormalCompleted", 0);
        PlayerPrefs.SetInt("isHardCompleted", 0);

        PlayerPrefs.SetInt("isEasyUnlocked", 0);
        PlayerPrefs.SetInt("isNormalUnlocked", 0);
        PlayerPrefs.SetInt("isHardUnlocked", 0);

        Debug.Log("All Jigsaw progress and notifications have been reset.");

        // บันทึกการเปลี่ยนแปลงใน PlayerPrefs
        PlayerPrefs.Save();

        // เรียกอัปเดตแกลเลอรีใหม่
        UpdateGallery();

        // แจ้ง UnlockNotificationUIManager ให้อัปเดตสถานะใหม่
        UnlockNotificationUIManager unlockManager = FindObjectOfType<UnlockNotificationUIManager>();
        if (unlockManager != null)
        {
            unlockManager.OnEnable(); // เรียกให้ตรวจสอบสถานะการแจ้งเตือนใหม่
        }
    }
}