using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class GrandpaController : MonoBehaviour
{
    private PlayerInput playerInput;
   [SerializeField] private float movementSpeed = 10f;
   [SerializeField] private float slowFallMultiplier = 0.5f; // ปัจจัยที่ใช้ลดความเร็วการตก
    
    private Rigidbody rb;
    private Vector2 movementInput; 
    private bool isGrounded;

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
    
}