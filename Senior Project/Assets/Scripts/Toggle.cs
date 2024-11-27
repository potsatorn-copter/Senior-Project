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
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogWarning("SoundManager not found in the scene.");
        }

        isMuted = false;

        // ลงทะเบียน AudioSource ที่ไม่ได้อยู่ใน SoundManager
        RegisterExternalAudioSources();

        // ส่งสถานะเสียงไปยัง SoundManager
        if (soundManager != null)
        {
            soundManager.MuteAllSounds(isMuted);
        }

        // อัปเดต UI ปุ่มให้ตรงกับสถานะเสียงที่โหลดมา
        UpdateMuteButtonUI();

        // เพิ่ม Listener ให้ปุ่ม
        muteButton.onClick.AddListener(ToggleMuteStatus);
    }

    public void ToggleMuteStatus()
    {
        isMuted = !isMuted;

        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateMuteButtonUI();

        if (soundManager != null)
        {
            soundManager.MuteAllSounds(isMuted);
        }
    }

    private void UpdateMuteButtonUI()
    {
        muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
    }

    private void RegisterExternalAudioSources()
    {
        AudioSource[] externalSources = FindObjectsOfType<AudioSource>();

        foreach (var audioSource in externalSources)
        {
            if (soundManager != null && !soundManager.externalAudioSources.Contains(audioSource))
            {
                soundManager.RegisterExternalAudioSource(audioSource);
            }
        }
    }
}