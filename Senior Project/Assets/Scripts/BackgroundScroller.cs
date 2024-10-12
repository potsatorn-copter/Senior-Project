using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public GameObject player; // ผู้เล่นที่จะใช้เป็น reference
    public float backgroundHeight = 18f; // ความสูงของภาพพื้นหลังในหน่วย Unity (ของภาพหนึ่งภาพ)
    public float triggerThreshold = 0.8f; // เลื่อนพื้นหลังก่อนที่จะพ้นภาพพื้นหลัง 90%

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // เก็บตำแหน่งเริ่มต้นของพื้นหลัง
    }

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นพ้นจาก threshold (เช่น 90% ของความสูงรวมของพื้นหลังทั้งสอง)
        if (player.transform.position.y > transform.position.y + (backgroundHeight * 2 * triggerThreshold))
        {
            RepositionBackground(); // ย้ายพื้นหลังขึ้นไปก่อนจะพ้นภาพพื้นหลังที่ 2
        }
    }

    private void RepositionBackground()
    {
        // ย้ายพื้นหลังขึ้นไปด้านบนโดยย้ายตามความสูงรวมของทั้งสองภาพ
        transform.position = new Vector3(transform.position.x, transform.position.y + (backgroundHeight * 2), transform.position.z);
    }
}