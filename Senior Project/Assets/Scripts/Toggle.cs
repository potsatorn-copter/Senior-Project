using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;

    private bool isMuted;

    private void Awake()
    {
        // ค้นหา SoundManager ที่เป็น Singleton จากซีนแรก
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogWarning("SoundManager not found in the scene.");
        }

        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        UpdateMuteButtonUI();

        muteButton.onClick.AddListener(MuteAllSounds);
    }

    public void MuteAllSounds()
    {
        isMuted = !isMuted;
        UpdateMuteButtonUI();

        if (soundManager != null)
        {
            soundManager.MuteAllSounds(isMuted);
        }
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
    }

    private void UpdateMuteButtonUI()
    {
        muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}