using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandpaController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveDirection = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1f; // เคลื่อนที่ไปทางซ้าย
            RotateCharacter(180f); // หมุนตัวละครให้หันหน้าไปทางซ้าย
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1f; // เคลื่อนที่ไปทางขวา
            RotateCharacter(0f); // หมุนตัวละครให้หันหน้าไปทางขวา
        }

        Vector3 movement = new Vector3(moveDirection * moveSpeed, rb.velocity.y, 0f);
        rb.velocity = movement;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void RotateCharacter(float yRotation)
    {
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}