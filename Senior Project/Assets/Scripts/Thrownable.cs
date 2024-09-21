using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thrownable : MonoBehaviour
{
    [Header("Throw Setting")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float baseThrowForce = 10f;
    public float maxThrowForce = 50f;
    public float holdTimeMultiplier = 10f;
    public float maxHoldTime = 2f;
    public bool showHoldTimeUpdate = true; 
    
    [Header("GUI")]
    public TextMeshProUGUI holdTimeText; 
    public Slider holdTimeSlider; 

    private float holdStartTime;

    void Update()
    {
        if (IsHoldingStart())
        {
            holdStartTime = Time.time;
            UpdateHoldTimeUI(0);
        }
        else if (IsHoldingEnd())
        {
            float throwForce = CalculateThrowForce();
            Vector2 position = Input.touchCount > 0 ? (Vector2)Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            ThrowProjectile(position, throwForce);
            UpdateHoldTimeUI(0);
        }
        else if (IsHolding())
        {
            UpdateHoldTimeUI(CalculateHoldDuration());
        }
    }

    bool IsHoldingStart()
    {
        return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    bool IsHoldingEnd()
    {
        return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }

    bool IsHolding()
    {
        return Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended);
    }

    float CalculateHoldDuration()
    {
        return Mathf.Min(Time.time - holdStartTime, maxHoldTime);
    }

    float CalculateThrowForce()
    {
        return Mathf.Min(baseThrowForce + CalculateHoldDuration() * holdTimeMultiplier, maxThrowForce);
    }

    void ThrowProjectile(Vector2 screenPosition, float throwForce)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.localRotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (Camera.main.ScreenToWorldPoint(screenPosition) - spawnPoint.position).normalized;// เพิ่มมุมในการโยน
            direction.y += 0.5f; // ปรับค่านี้เพื่อเพิ่มความสูงที่ต้องการ
            direction.Normalize(); // ทำให้ทิศทางมีความยาว 1

            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
            
            /*Vector2 direction = (Camera.main.ScreenToWorldPoint(screenPosition) - spawnPoint.position).normalized;
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);*/
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
}
