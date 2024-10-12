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

    // Function to detect collision with other objects
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object the player collided with has the tag "TrampolinePlatform"
        if (collision.gameObject.CompareTag("TrampolinePlatform"))
        {
            // Iterate through all contact points to check if the collision was from the top
            foreach (ContactPoint contact in collision.contacts)
            {
                // Check if the contact point normal is pointing upwards (indicating a top collision)
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
                {
                    // Play the jump sound when the player hits the top of the platform
                    SoundManager.instance.Play(SoundManager.SoundName.Jump);
                    break;
                }
            }
        }
    }

    // ฟังก์ชันเก็บไอเทมกล้วย
    public void CollectBanana()
    {
        hasJumpBoost = true; // ตั้งค่าการบูสต์เมื่อเก็บกล้วย
    }
}