using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thrownable : MonoBehaviour
{
    public static Thrownable Instance; // สร้าง instance ของ Thrownable

    private void Awake()
    {
        // ตรวจสอบว่ามี Instance แล้วหรือไม่
        if (Instance == null)
        {
            Instance = this; // กำหนด instance เป็น this เมื่อสร้าง
        }
        else
        {
            Destroy(gameObject); // ป้องกันการสร้าง instance ซ้ำ
        }
    }

    [Header("Throw Setting")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float baseThrowForce = 10f;
    public float maxThrowForce = 50f;
    public float holdTimeMultiplier = 10f;
    public float maxHoldTime = 2f;
    public float chargeDecreaseRate = 1f; // อัตราที่ลดลงเมื่อกดปุ่มเพื่อลด
    public bool showHoldTimeUpdate = true; 
    
    [Header("Trajectory Line")]
    public LineRenderer trajectoryLine; // เส้นแสดงทิศทางการปา
    public int lineSegmentCount = 20;   // จำนวนจุดของเส้น

    [Header("GUI")]
    public TextMeshProUGUI holdTimeText;
    public Slider holdTimeSlider;

    private float holdDuration = 0f;  // ระยะเวลาที่ชาร์จ
    private bool canThrow = true;     // ตัวแปรบอกว่าปาได้หรือไม่

    void Update()
    {
        if (Time.timeScale == 0f) return; // ถ้าเกมหยุด ให้หยุดการทำงานของสคริปต์นี้เช่นกัน

        if (canThrow) // ถ้าสามารถปาได้
        {
            if (IsHoldingStart())
            {
                // เพิ่มค่าชาร์จเมื่อกดปุ่ม
                holdDuration += chargeDecreaseRate * Time.deltaTime; 
                holdDuration = Mathf.Min(holdDuration, maxHoldTime); // ห้ามเกินค่า maxHoldTime
            }
            else if (IsHoldingEnd())
            {
                // ปาเมื่อปล่อยปุ่ม
                float throwForce = CalculateThrowForce();
                Vector2 position = Input.touchCount > 0 ? (Vector2)Input.GetTouch(0).position : (Vector2)Input.mousePosition;
                ThrowProjectile(position, throwForce);

                // รีเซ็ตค่าเกจชาร์จ
                holdDuration = 0f; 
                UpdateHoldTimeUI(holdDuration);
                trajectoryLine.enabled = false; // ซ่อนเส้นเมื่อปาแล้ว

                // ป้องกันการปารัวๆ
                canThrow = false; // ปาหลังจากนี้ไม่ได้จนกว่าจะชน
            }

            // วาดเส้นแสดงทิศทาง
            if (holdDuration > 0)
            {
                UpdateHoldTimeUI(holdDuration);
                DrawTrajectory(CalculateThrowForce()); // วาดเส้นบอกความแรง
            }
        }
    }

    bool IsHoldingStart()
    {
        return Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary);
    }

    bool IsHoldingEnd()
    {
        return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }

    float CalculateThrowForce()
    {
        return Mathf.Min(baseThrowForce + holdDuration * holdTimeMultiplier, maxThrowForce);
    }

    void ThrowProjectile(Vector2 screenPosition, float throwForce)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.localRotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (Camera.main.ScreenToWorldPoint(screenPosition) - spawnPoint.position).normalized;
            direction.y += 0.5f; // ปรับค่าความสูงของการปา
            direction.Normalize();

            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        }
    }

    void UpdateHoldTimeUI(float holdDuration)
    {
        if (showHoldTimeUpdate)
        {
            // คำนวณค่าความแรงปัจจุบัน
            float currentForce = Mathf.Min(baseThrowForce + holdDuration * holdTimeMultiplier, maxThrowForce);
            
            if (holdTimeText != null)
                holdTimeText.text = $"POWER : {currentForce:F2}";

            // อัพเดต Slider
            if (holdTimeSlider != null)
                holdTimeSlider.value = holdDuration / maxHoldTime; // ปรับ Slider ให้สัมพันธ์กับ holdDuration
        }
    }

    void DrawTrajectory(float throwForce)
    {
        trajectoryLine.enabled = true;  // เปิดใช้งาน LineRenderer
        Vector3[] points = new Vector3[lineSegmentCount];
        Vector2 startPoint = spawnPoint.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - spawnPoint.position).normalized;
        direction.y += 0.5f;  // ปรับเพื่อควบคุมแนวโค้ง

        float timeStep = 0.1f; // ระยะเวลาที่ใช้ระหว่างแต่ละจุด
        float maxTime = 1.0f; // ระยะเวลาสูงสุดที่ต้องการให้เส้นยาว (ปรับค่านี้ตามที่ต้องการ)

        // คำนวณแต่ละจุดตามเวลาของ trajectory
        for (int i = 0; i < lineSegmentCount; i++)
        {
            float time = i * timeStep;
            if (time > maxTime) break; // จำกัดความยาวของเส้น
            points[i] = startPoint + (direction * throwForce * time) + 0.5f * Physics2D.gravity * time * time;
        }

        trajectoryLine.positionCount = Mathf.Min(lineSegmentCount, (int)(maxTime / timeStep));
        trajectoryLine.SetPositions(points);
    }

    // ฟังก์ชันนี้ให้ Bottle เรียกเมื่อขวดชนเพื่ออนุญาตให้ปาได้อีกครั้ง
    public void EnableThrowingAgain()
    {
        canThrow = true;
    }
}