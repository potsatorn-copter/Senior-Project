using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Thrownable : MonoBehaviour
{
    public static Thrownable Instance;

    [Header("Throw Setting")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float baseThrowForce = 10f;
    public float maxThrowForce = 50f;
    public float holdTimeMultiplier = 10f;
    public float maxHoldTime = 2f;
    public bool showHoldTimeUpdate = true;

    [Header("Trajectory Line")]
    public LineRenderer trajectoryLine;
    public int lineSegmentCount = 20;

    [Header("GUI")]
    public TextMeshProUGUI holdTimeText;
    public Slider holdTimeSlider;

    private float holdDuration = 0f;
    private bool isCharging = false;  // ตัวแปรบอกว่ากำลังชาร์จอยู่
    private bool canThrow = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Time.timeScale == 0f || !isCharging) return;  // ถ้าเกมหยุดหรือไม่ได้ชาร์จ ให้หยุดทำงาน

        // เพิ่มระยะเวลาชาร์จ
        holdDuration += Time.deltaTime;
        holdDuration = Mathf.Min(holdDuration, maxHoldTime);  // จำกัดไม่ให้เกิน maxHoldTime

        UpdateHoldTimeUI(holdDuration);  // อัปเดต UI
        DrawTrajectory(CalculateThrowForce());  // วาดเส้นแสดงทิศทาง
    }

    public void StartCharging()
    {
        if (canThrow)
        {
            isCharging = true;  // เริ่มชาร์จ
            holdDuration = 0f;  // รีเซ็ตเวลา
            trajectoryLine.enabled = true;  // แสดงเส้นทิศทาง
        }
    }

    public void StopChargingAndThrow()
    {
        if (canThrow)
        {
            float throwForce = CalculateThrowForce();
            Vector2 position = Input.mousePosition;
            ThrowProjectile(position, throwForce);

            holdDuration = 0f;
            UpdateHoldTimeUI(holdDuration);
            trajectoryLine.enabled = false;  // ซ่อนเส้น
            isCharging = false;  // หยุดชาร์จ
            canThrow = false;  // ปาไม่ได้จนกว่าจะชน
        }
    }

    float CalculateThrowForce()
    {
        return Mathf.Min(baseThrowForce + holdDuration * holdTimeMultiplier, maxThrowForce);
    }

    void ThrowProjectile(Vector2 screenPosition, float throwForce)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (Camera.main.ScreenToWorldPoint(screenPosition) - spawnPoint.position).normalized;
            direction.y += 0.5f;
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        }
    }

    void UpdateHoldTimeUI(float holdDuration)
    {
        if (showHoldTimeUpdate)
        {
            float currentForce = Mathf.Min(baseThrowForce + holdDuration * holdTimeMultiplier, maxThrowForce);

            if (holdTimeText != null)
                holdTimeText.text = $"POWER : {currentForce:F2}";

            if (holdTimeSlider != null)
                holdTimeSlider.value = holdDuration / maxHoldTime;
        }
    }

    void DrawTrajectory(float throwForce)
    {
        Vector3[] points = new Vector3[lineSegmentCount];
        Vector2 startPoint = spawnPoint.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - spawnPoint.position).normalized;
        direction.y += 0.5f;

        float timeStep = 0.1f;
        for (int i = 0; i < lineSegmentCount; i++)
        {
            float time = i * timeStep;
            points[i] = startPoint + (direction * throwForce * time) + 0.5f * Physics2D.gravity * time * time;
        }

        trajectoryLine.positionCount = lineSegmentCount;
        trajectoryLine.SetPositions(points);
    }

    public void EnableThrowingAgain()
    {
        canThrow = true;
    }
}
