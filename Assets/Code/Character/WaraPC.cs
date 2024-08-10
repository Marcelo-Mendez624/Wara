using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 200f;
    public float dashDuration = 0.1f; // Duración del dash
    public float dashCooldown = 0.5f; // Tiempo de espera entre dashes
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
     private float dashTime;
    private float dashCooldownTimer;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        HandleDash();
    }

    void Move()
    {
        if(isDashing) return;

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);  // Flip character sprite to the right
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1); // Flip character sprite to the left
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = true;
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    void HandleDash()
    {
        dashCooldownTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire3") && dashCooldownTimer >= dashCooldown && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;

            if (dashTime <= 0)
            {
                EndDash();
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashCooldownTimer = 0f;

        // Reset velocity before applying force
        rb.velocity = Vector2.zero;

        // Apply a force in the direction the character is facing
        rb.AddForce(new Vector2(transform.localScale.x * dashForce, 0f), ForceMode2D.Impulse);
    }

    void EndDash()
    {
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}