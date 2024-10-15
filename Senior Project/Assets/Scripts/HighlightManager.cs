using UnityEngine;
using UnityEngine.UI;

public class HighlightManager : MonoBehaviour
{
    public Button[] buttons;  // อาร์เรย์ของปุ่มทั้ง 4 ช้อย
    public Vector3 pressedScale = new Vector3(0.95f, 0.95f, 1f);  // ขนาดเมื่อถูกกด
    public Vector3 originalScale = new Vector3(1f, 1f, 1f);  // ขนาดเดิม
    public Color pressedColor = Color.yellow;  // สีเมื่อปุ่มถูกกด
    public Color originalColor = Color.white;  // สีเดิมของปุ่ม
    public ScoreManagerQuiz scoreManager;  // อ้างอิงถึง ScoreManager
    public int questionIndex;  // เก็บ index ของคำถามนี้
    public Button nextButton;  // ปุ่ม Next ที่จะถูกแสดงเมื่อเลือกช้อยแล้ว

    private Button selectedButton;  // ปุ่มที่ถูกเลือกตอนนี้

    void Start()
    {
        // ซ่อนปุ่ม Next ในตอนเริ่มต้น
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
        }

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));  // เพิ่ม listener ให้กับทุกปุ่ม
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        if (selectedButton != clickedButton)
        {
            // ถ้ามีปุ่มที่ถูกเลือกอยู่แล้ว ให้กลับสู่สภาพเดิม
            if (selectedButton != null)
            {
                selectedButton.transform.localScale = originalScale;  // คืนค่าขนาดเดิม

                // คืนค่าสีเดิมของปุ่ม
                ColorBlock originalCb = selectedButton.colors;
                originalCb.normalColor = originalColor;
                originalCb.highlightedColor = originalColor;
                originalCb.pressedColor = originalColor;
                originalCb.selectedColor = originalColor;
                selectedButton.colors = originalCb;
            }

            // ตั้งค่าปุ่มที่ถูกคลิกให้ยุบลง
            clickedButton.transform.localScale = pressedScale;
            SoundManager.instance.Play(SoundManager.SoundName.Click);

            // เปลี่ยนสีปุ่มเป็นสีเหลืองเข้มเมื่อถูกกด
            ColorBlock clickedCb = clickedButton.colors;
            clickedCb.normalColor = pressedColor;
            clickedCb.highlightedColor = pressedColor;
            clickedCb.pressedColor = pressedColor;
            clickedCb.selectedColor = pressedColor;
            clickedButton.colors = clickedCb;

            // เก็บปุ่มที่ถูกคลิกเป็นปุ่มที่ถูกเลือกตอนนี้
            selectedButton = clickedButton;

            // อัพเดตคะแนนตาม Tag ของปุ่ม
            int score = 0;
            if (clickedButton.CompareTag("Score1")) score = 1;
            else if (clickedButton.CompareTag("Score2")) score = 2;
            else if (clickedButton.CompareTag("Score3")) score = 3;
            else if (clickedButton.CompareTag("Score4")) score = 4;

            scoreManager.SetScore(questionIndex, score);

            // แสดงปุ่ม Next เมื่อมีการเลือกช้อย
            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(true);
            }
        }
    }
}