using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thrownable : MonoBehaviour
{
    /*public Vector3 Initial_Position;
    public int Bottle_Speed;

    public void Awake()
    {
        Initial_Position = transform.position;
    }
    public void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void OnMouseUp()
    {
        Vector2 Spring_force = Initial_Position - transform.position;
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<Rigidbody2D>().AddForce(Bottle_Speed * Spring_force);
    }

    public void OnMouseDrag()
    {
        Vector3 DragPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = new Vector3(DragPosition.x, DragPosition.y);
    }

    public void OnTap()
    {
        ///////////////
    }*/
    
    /*public GameObject projectilePrefab;
    public Transform firePoint;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // หรือปุ่มอื่นๆที่คุณต้องการใช้
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }*/

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
            Vector2 direction = (Camera.main.ScreenToWorldPoint(screenPosition) - spawnPoint.position).normalized;
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        }
    }

    void UpdateHoldTimeUI(float holdDuration)
    {
        if (showHoldTimeUpdate)
        {
            float currentForce = Mathf.Min(baseThrowForce + holdDuration * holdTimeMultiplier, maxThrowForce);

            if (holdTimeText != null)
                holdTimeText.text = $"Force: {currentForce:F2}";

            if (holdTimeSlider != null)
                holdTimeSlider.value = holdDuration / maxHoldTime;
        }
    }
}
