using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class GrandpaController : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float slowFallMultiplier = 0.5f;
    
    private Rigidbody rb;
    private Vector2 movementInput; 

    public bool hasJumpBoost = false; // สถานะการบูสต์การกระโดดจากกล้วย
    public float boostMultiplier = 1.5f; // ตัวคูณแรงกระโดดเมื่อมีบูสต์

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // Get movement input from the Input System
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Apply horizontal movement
        Vector3 velocity = rb.velocity;
        velocity.x = movementInput.x * movementSpeed;
        rb.velocity = velocity;

        // Rotate character based on movement direction
        if (movementInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Facing right
        }
        else if (movementInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Facing left
        }

        // Apply slow fall effect when the character is falling
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.up * slowFallMultiplier, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ตรวจสอบว่าแพลตฟอร์มที่ชนคือ TrampolinePlatform หรือ FakePlatform
        TrampolinePlatform trampolinePlatform = collision.gameObject.GetComponent<TrampolinePlatform>();
        FakePlatform fakePlatform = collision.gameObject.GetComponent<FakePlatform>();

        if (trampolinePlatform != null && !trampolinePlatform.isSteppedOn)
        {
            // กรณี TrampolinePlatform และยังไม่ได้เหยียบ
            ScoremanagerScene2.Instance.AddScore(100); // เพิ่มคะแนน
            trampolinePlatform.isSteppedOn = true; // บันทึกว่าแพลตฟอร์มถูกเหยียบแล้ว

            // เล่นเสียงเมื่อเหยียบแพลตฟอร์ม
            SoundManager.instance.Play(SoundManager.SoundName.Jump);
        }
        else if (fakePlatform != null && !fakePlatform.isSteppedOn)
        {
            // กรณี FakePlatform และยังไม่ได้เหยียบ
            ScoremanagerScene2.Instance.AddScore(100); // เพิ่มคะแนน
            fakePlatform.isSteppedOn = true; // บันทึกว่าแพลตฟอร์มถูกเหยียบแล้ว

            // เล่นเสียงเมื่อเหยียบแพลตฟอร์ม
            SoundManager.instance.Play(SoundManager.SoundName.Jump);
        }
    }

    // ฟังก์ชันตรวจจับการชนกับไอเทมพิเศษ เช่น ดาวหรือแอปเปิ้ล
    private void OnTriggerEnter(Collider other)
    {
        // เมื่อชนกับดาว เพิ่ม 100 คะแนน
        if (other.CompareTag("Star"))
        {
            ScoremanagerScene2.Instance.AddScore(100); // เพิ่มคะแนน 100
            SoundManager.instance.Play(SoundManager.SoundName.Eat);
            Destroy(other.gameObject); // ทำลายไอเทมหลังเก็บได้
        }
        // เมื่อชนกับแอปเปิ้ล เพิ่ม 50 คะแนน
        else if (other.CompareTag("Apple"))
        {
            ScoremanagerScene2.Instance.AddScore(50); // เพิ่มคะแนน 50
            SoundManager.instance.Play(SoundManager.SoundName.Eat);
            Destroy(other.gameObject); // ทำลายไอเทมหลังเก็บได้
        }
    }

    // ฟังก์ชันเก็บไอเทมกล้วย
    public void CollectBanana()
    {
        hasJumpBoost = true; // ตั้งค่าการบูสต์เมื่อเก็บกล้วย
    }
}