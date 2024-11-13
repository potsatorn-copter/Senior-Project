using System.Collections;
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
            // เริ่มต้นด้วยการแสดง lockedImage (ไม่ให้กดปุ่มเมื่อยังไม่ครบ)
            galleryImage.sprite = jigsawImage.lockedImage;

            // เพิ่ม Listener สำหรับการกดปุ่มเพื่อให้เกิดการปลดล็อค
            Button galleryButton = galleryImage.GetComponent<Button>();
            if (galleryButton != null)
            {
                // ลบ Listener เดิมเพื่อป้องกันการเรียกซ้ำ
                galleryButton.onClick.RemoveAllListeners();

                // เพิ่ม Listener ใหม่
                galleryButton.onClick.AddListener(() =>
                {
                    if (jigsawImage.IsComplete()) // เช็คว่ารูปภาพครบแล้วหรือยัง
                    {
                        // เล่นแอนิเมชันเมื่อปลดล็อค
                        Animator animator = galleryImage.GetComponent<Animator>();
                        if (animator != null)
                        {
                            // ตั้ง Trigger สำหรับการเริ่มเล่นอนิเมชัน
                            animator.SetTrigger("Unlock");

                            // ทำการเปลี่ยนภาพเป็น completedImage หลังจากอนิเมชันเสร็จ
                            StartCoroutine(WaitForAnimation(animator, galleryImage, jigsawImage));

                            // แสดงการแจ้งเตือนเมื่อปลดล็อค
                            ShowUnlockNotification(jigsawImage);
                        }
                    }
                    else
                    {
                        // รูปภาพยังไม่ครบ แสดงข้อความเตือน
                        Debug.Log("Jigsaw image is not complete yet.");

                        // Optional: You can play an animation or change the sprite to show an alert (optional)
                        // For now, the sprite remains lockedImage if incomplete
                    }
                });
            }
        }
    }

    // Coroutine เพื่อรอจนกว่าอนิเมชันจะเสร็จสิ้น
    private IEnumerator WaitForAnimation(Animator animator, Image galleryImage, JigsawImage jigsawImage)
    {
        // รอจนกว่าอนิเมชันจะเสร็จ
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // เปลี่ยนภาพเป็น completedImage หลังจากอนิเมชันเสร็จ
        galleryImage.sprite = jigsawImage.completedImage;
    }

    // ฟังก์ชันที่แสดงการแจ้งเตือนเมื่อปลดล็อคภาพ
    private void ShowUnlockNotification(JigsawImage jigsawImage)
    {
        // ตรวจสอบว่าได้แสดงการแจ้งเตือนแล้วหรือยัง
        if (jigsawImage.imageName == "ImageEasy" && PlayerPrefs.GetInt("hasShownEasyUnlockNotification", 0) == 0)
        {
            Debug.Log("Easy image unlocked!");
            PlayerPrefs.SetInt("hasShownEasyUnlockNotification", 1);
            // อาจจะเรียก UI ที่แสดงการแจ้งเตือนที่นี่
        }
        else if (jigsawImage.imageName == "ImageNormal" && PlayerPrefs.GetInt("hasShownNormalUnlockNotification", 0) == 0)
        {
            Debug.Log("Normal image unlocked!");
            PlayerPrefs.SetInt("hasShownNormalUnlockNotification", 1);
            // อาจจะเรียก UI ที่แสดงการแจ้งเตือนที่นี่
        }
        else if (jigsawImage.imageName == "ImageHard" && PlayerPrefs.GetInt("hasShownHardUnlockNotification", 0) == 0)
        {
            Debug.Log("Hard image unlocked!");
            PlayerPrefs.SetInt("hasShownHardUnlockNotification", 1);
            // อาจจะเรียก UI ที่แสดงการแจ้งเตือนที่นี่
        }
        PlayerPrefs.Save();
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

        PlayerPrefs.Save();
        Debug.Log("Progress reset.");
    }
}
