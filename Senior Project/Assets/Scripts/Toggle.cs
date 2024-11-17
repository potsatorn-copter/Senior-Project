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

        // โหลดสถานะการปิด/เปิดเสียงจาก PlayerPrefs
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

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
        // สลับสถานะเสียง
        isMuted = !isMuted;

        // บันทึกสถานะเสียงลง PlayerPrefs
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // อัปเดต UI
        UpdateMuteButtonUI();

        // ส่งคำสั่งปิด/เปิดเสียงไปยัง SoundManager
        if (soundManager != null)
        {
            soundManager.MuteAllSounds(isMuted);
        }
    }

    private void UpdateMuteButtonUI()
    {
        // เปลี่ยน Sprite ของปุ่มให้ตรงกับสถานะเสียง
        muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}