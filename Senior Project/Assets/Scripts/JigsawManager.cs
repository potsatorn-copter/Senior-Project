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

    public void UpdateGallery()
    {
        if (IsGalleryScene())
        {
            GameObject easyImageObj = GameObject.Find("EasyImage");
            GameObject normalImageObj = GameObject.Find("NormalImage");
            GameObject hardImageObj = GameObject.Find("HardImage");

            if (easyImageObj && normalImageObj && hardImageObj)
            {
                UpdateSingleGalleryImage(easyImageObj.GetComponent<Image>(), jigsawImages[0]);
                UpdateSingleGalleryImage(normalImageObj.GetComponent<Image>(), jigsawImages[1]);
                UpdateSingleGalleryImage(hardImageObj.GetComponent<Image>(), jigsawImages[2]);
            }
            else
            {
                Debug.LogWarning("One or more gallery images are missing in the Gallery Scene.");
            }
        }
    }

    private void UpdateSingleGalleryImage(Image galleryImage, JigsawImage jigsawImage)
    {
        if (galleryImage != null)
        {
            // แสดง lockedImage เป็นค่าเริ่มต้น
            galleryImage.sprite = jigsawImage.lockedImage;

            // เพิ่ม Listener สำหรับการกดเพื่อให้เกิดการปลดล็อค
            Button galleryButton = galleryImage.GetComponent<Button>();
            if (galleryButton != null)
            {
                // ลบ Listener เดิมเพื่อป้องกันการเรียกซ้ำ
                galleryButton.onClick.RemoveAllListeners();

                // เพิ่ม Listener ใหม่
                galleryButton.onClick.AddListener(() =>
                {
                    if (jigsawImage.IsComplete())
                    {
                        // เล่นแอนิเมชันเมื่อปลดล็อค
                        Animator animator = galleryImage.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.Play("UnlockAnimation"); // เรียกใช้ Animation
                        }

                        // เปลี่ยนภาพเป็น completedImage
                        galleryImage.sprite = jigsawImage.completedImage;
                    }
                    else
                    {
                        Debug.Log("Jigsaw image is not complete yet.");
                    }
                });
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

                        // บันทึกสถานะการปลดล็อคใน PlayerPrefs
                        if (currentJigsaw.imageName == "ImageEasy")
                        {
                            PlayerPrefs.SetInt("isEasyUnlocked", 1);
                            Debug.Log("isEasyUnlocked set to 1");
                        }
                        else if (currentJigsaw.imageName == "ImageNormal")
                        {
                            PlayerPrefs.SetInt("isNormalUnlocked", 1);
                            Debug.Log("isNormalUnlocked set to 1");
                        }
                        else if (currentJigsaw.imageName == "ImageHard")
                        {
                            PlayerPrefs.SetInt("isHardUnlocked", 1);
                            Debug.Log("isHardUnlocked set to 1");
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

        // รีเซ็ตสถานะการแสดงแจ้งเตือนเพื่อให้สามารถแสดงการแจ้งเตือนใหม่ได้
        PlayerPrefs.SetInt("hasShownEasyUnlockNotification", 0);
        PlayerPrefs.SetInt("hasShownNormalUnlockNotification", 0);
        PlayerPrefs.SetInt("hasShownHardUnlockNotification", 0);
        Debug.Log("Notification status has been reset.");

        // รีเซ็ตสถานะการปลดล็อคใน PlayerPrefs
        PlayerPrefs.SetInt("isEasyUnlocked", 0);
        PlayerPrefs.SetInt("isNormalUnlocked", 0);
        PlayerPrefs.SetInt("isHardUnlocked", 0);

        // สั่ง Save เพียงครั้งเดียวเพื่อบันทึกการเปลี่ยนแปลงทั้งหมด
        PlayerPrefs.Save();

        UpdateGallery();
    }
}