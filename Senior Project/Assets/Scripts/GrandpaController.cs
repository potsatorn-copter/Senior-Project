using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GrandpaController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float maxFallSpeed = -10f; // ความเร็วตกสูงสุด (ค่าเป็นลบ)

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private SpriteRenderer spriteRenderer;

    private ScoremanagerScene2 scoreManager; // ตัวแปรเก็บอ้างอิงถึง ScoremanagerScene2

    public bool hasJumpBoost = false; // สถานะการบูสต์การกระโดดจากกล้วย
    public float boostMultiplier = 1.5f; // ตัวคูณแรงกระโดดเมื่อมีบูสต์

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject.");
        }

        // ค้นหา ScoremanagerScene2 ในซีนปัจจุบัน
        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }
    }

    private void Update()
    {
        // ตรวจจับการกดปุ่มซ้าย
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopMoving();
        }

        // ตรวจจับการกดปุ่มขวา
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopMoving();
        }

        // ตั้งค่าการเคลื่อนไหวตามทิศทางที่กำหนด
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
            movementInput = Vector2.zero; // หยุดเคลื่อนไหวเมื่อไม่มีการกดปุ่ม
        }
    }

    private void FixedUpdate()
    {
        // Apply horizontal movement
        Vector2 velocity = rb.velocity;
        velocity.x = movementInput.x * movementSpeed;

        // จำกัดความเร็วตก
        if (velocity.y < maxFallSpeed)
        {
            velocity.y = maxFallSpeed; // จำกัดความเร็วในแนวดิ่ง
        }

        rb.velocity = velocity;

        // Flip Sprite based on movement direction
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
        isMovingLeft = true; // ตั้งค่าการเดินซ้าย
        isMovingRight = false; // ปิดการเดินขวา
    }

    public void MoveRight()
    {
        isMovingRight = true; // ตั้งค่าการเดินขวา
        isMovingLeft = false; // ปิดการเดินซ้าย
    }

    public void StopMoving()
    {
        isMovingLeft = false; // หยุดเดินซ้าย
        isMovingRight = false; // หยุดเดินขวา
        movementInput = Vector2.zero; // รีเซ็ต movementInput
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบว่าแพลตฟอร์มที่ชนคือ TrampolinePlatform หรือ FakePlatform
        TrampolinePlatform trampolinePlatform = collision.gameObject.GetComponent<TrampolinePlatform>();
        FakePlatform fakePlatform = collision.gameObject.GetComponent<FakePlatform>();

        if (trampolinePlatform != null && !trampolinePlatform.isSteppedOn)
        {
            if (scoreManager != null) scoreManager.AddScore(100); // เพิ่มคะแนน
            trampolinePlatform.isSteppedOn = true;

            SoundManager.instance.Play(SoundManager.SoundName.Jump);
        }
        else if (fakePlatform != null && !fakePlatform.isSteppedOn)
        {
            if (scoreManager != null) scoreManager.AddScore(100); // เพิ่มคะแนน
            fakePlatform.isSteppedOn = true;

            SoundManager.instance.Play(SoundManager.SoundName.Jump);
        }
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

    public void CollectBanana()
    {
        hasJumpBoost = true; // ตั้งค่าการบูสต์เมื่อเก็บกล้วย
    }
}