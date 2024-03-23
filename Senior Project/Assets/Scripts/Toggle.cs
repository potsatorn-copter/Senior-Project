using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;

    private bool isMuted;

    private void Awake()
    {
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        UpdateMuteButtonUI();

        muteButton.onClick.AddListener(MuteAllSounds);
    }

    public void MuteAllSounds()
    {
        isMuted = !isMuted;
        UpdateMuteButtonUI();

        soundManager.MuteAllSounds(isMuted);
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
    }

    private void UpdateMuteButtonUI()
    {
        muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}

