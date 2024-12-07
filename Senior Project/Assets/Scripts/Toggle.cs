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

        // ใช้สถานะเสียงจาก SoundManager หากมีอยู่
        if (soundManager != null)
        {
            isMuted = soundManager.IsMuted; // ใช้สถานะเสียงจาก SoundManager
        }
        else
        {
            isMuted = false; // ค่าเริ่มต้นเป็นเปิดเสียง
        }

        // ลงทะเบียน AudioSource ที่ไม่ได้อยู่ใน SoundManager
        RegisterExternalAudioSources();

        // ส่งสถานะเสียงไปยัง SoundManager
        if (soundManager != null)
        {
            soundManager.MuteAllSounds(isMuted);
        }

        // อัปเดต UI ปุ่มให้ตรงกับสถานะเสียงปัจจุบัน
        UpdateMuteButtonUI();

        // เพิ่ม Listener ให้ปุ่ม
        muteButton.onClick.AddListener(ToggleMuteStatus);
    }

    public void ToggleMuteStatus()
    {
        isMuted = !isMuted;

        // บันทึกสถานะเสียงใน SoundManager
        if (soundManager != null)
        {
            soundManager.IsMuted = isMuted;
        }

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
