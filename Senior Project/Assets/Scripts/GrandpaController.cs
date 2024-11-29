using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GrandpaController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float maxFallSpeed = -10f;
    [SerializeField] private float fallSmoothDuration = 1f;
    [SerializeField] private float maxFallSpeedSmooth = -5f;
    private Coroutine fallSmoothCoroutine;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private SpriteRenderer spriteRenderer;

    private ScoremanagerScene2 scoreManager;

    public bool hasJumpBoost = false;
    public float boostMultiplier = 1.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject.");
        }

        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            StopMoving();
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            StopMoving();
        }

        if (isMovingLeft)
        {
            movementInput = Vector2.left;
        }
        else if (isMovingRight)
        {
            movementInput = Vector2.right;
        }
        else
        {
            movementInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movementInput.x * movementSpeed;

        if (velocity.y < maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
        }

        rb.velocity = velocity;
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (movementInput.x > 0) // เดินขวา
        {
            spriteRenderer.flipX = false; // หันหน้าขวา
        }
        else if (movementInput.x < 0) // เดินซ้าย
        {
            spriteRenderer.flipX = true; // หันหน้าซ้าย
        }
    }

    public void MoveLeft()
    {
        isMovingLeft = true;
        isMovingRight = false;
    }

    public void MoveRight()
    {
        isMovingRight = true;
        isMovingLeft = false;
    }

    public void StopMoving()
    {
        isMovingLeft = false;
        isMovingRight = false;
        movementInput = Vector2.zero;
    }

    public void StartSmoothFall()
    {
        if (fallSmoothCoroutine != null)
        {
            StopCoroutine(fallSmoothCoroutine);
        }
        fallSmoothCoroutine = StartCoroutine(SmoothFall());
    }

    private IEnumerator SmoothFall()
    {
        float elapsedTime = 0f;
        float startFallSpeed = rb.velocity.y;

        while (elapsedTime < fallSmoothDuration)
        {
            elapsedTime += Time.deltaTime;
            float newFallSpeed = Mathf.Lerp(startFallSpeed, maxFallSpeedSmooth, elapsedTime / fallSmoothDuration);
            rb.velocity = new Vector2(rb.velocity.x, newFallSpeed);
            yield return null;
        }

        rb.velocity = new Vector2(rb.velocity.x, maxFallSpeedSmooth);
    }
    public void CollectBanana()
    {
        hasJumpBoost = true; // ตั้งค่าการบูสต์เมื่อเก็บกล้วย
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // เมื่อชนกับดาว เพิ่ม 100 คะแนน
        if (other.CompareTag("Star"))
        {
            if (scoreManager != null) scoreManager.AddScore(100); // เพิ่มคะแนน 100
            SoundManager.instance.Play(SoundManager.SoundName.Eat);
            Destroy(other.gameObject); // ทำลายไอเทมหลังเก็บได้
        }
        // ตรวจจับไอเทมพิเศษ เช่น ดาว
        if (other.CompareTag("Star2"))
        {
            if (scoreManager != null)
            {
                scoreManager.AddScore(100); // เพิ่มคะแนน 100
                Destroy(other.gameObject); // ทำลายไอเทมหลังเก็บได้
                SoundManager.instance.Play(SoundManager.SoundName.Eat);
                scoreManager.EndGameWithJigsawCheck(); // เรียกใช้เมธอดใหม่เพื่อตรวจสอบการจบเกม
            }
            
        }

        // เมื่อชนกับแอปเปิ้ล เพิ่ม 50 คะแนน
        else if (other.CompareTag("Apple"))
        {
            if (scoreManager != null) scoreManager.AddScore(50); // เพิ่มคะแนน 50
            SoundManager.instance.Play(SoundManager.SoundName.Eat);
            Destroy(other.gameObject); // ทำลายไอเทมหลังเก็บได้
        }
    }


}