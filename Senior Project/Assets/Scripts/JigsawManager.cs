using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
            galleryImage.sprite = jigsawImage.IsComplete() ? jigsawImage.completedImage : jigsawImage.lockedImage;
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
                    SaveJigsawProgress(currentJigsaw);

                    if (currentJigsaw.IsComplete())
                    {
                        // บันทึกสถานะการปลดล็อคใน PlayerPrefs
                        if (currentJigsaw.imageName == "ImageEasy")
                        {
                            PlayerPrefs.SetInt("isEasyUnlocked", 1);
                        }
                        else if (currentJigsaw.imageName == "ImageNormal")
                        {
                            PlayerPrefs.SetInt("isNormalUnlocked", 1);
                        }
                        else if (currentJigsaw.imageName == "ImageHard")
                        {
                            PlayerPrefs.SetInt("isHardUnlocked", 1);
                        }
                        PlayerPrefs.Save();
                    }
                }
            }
        }
    }

    private void SaveJigsawProgress(JigsawImage jigsawImage)
    {
        foreach (var piece in jigsawImage.jigsawPieces)
        {
            piece.isCollected = true;
        }
    }

    private void LoadJigsawProgress(JigsawImage jigsawImage)
    {
        bool isUnlocked = false;
        
        // ตรวจสอบสถานะการปลดล็อคจาก PlayerPrefs
        if (jigsawImage.imageName == "ImageEasy")
        {
            isUnlocked = PlayerPrefs.GetInt("isEasyUnlocked", 0) == 1;
        }
        else if (jigsawImage.imageName == "ImageNormal")
        {
            isUnlocked = PlayerPrefs.GetInt("isNormalUnlocked", 0) == 1;
        }
        else if (jigsawImage.imageName == "ImageHard")
        {
            isUnlocked = PlayerPrefs.GetInt("isHardUnlocked", 0) == 1;
        }

        foreach (var piece in jigsawImage.jigsawPieces)
        {
            piece.isCollected = isUnlocked;
        }
    }

    private bool IsGalleryScene()
    {
        return SceneManager.GetActiveScene().name == "MemoryGallery";
    }

    public void ResetJigsawProgress()
    {
        foreach (var jigsawImage in jigsawImages)
        {
            foreach (var piece in jigsawImage.jigsawPieces)
            {
                piece.isCollected = false;
            }
        }

        // รีเซ็ตสถานะการปลดล็อคใน PlayerPrefs
        PlayerPrefs.SetInt("isEasyUnlocked", 0);
        PlayerPrefs.SetInt("isNormalUnlocked", 0);
        PlayerPrefs.SetInt("isHardUnlocked", 0);
        PlayerPrefs.Save();

        UpdateGallery();
    }
}