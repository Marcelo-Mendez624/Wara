using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathState
{
    Time,
    Dead
}

public class WaraPC : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 200f;
    public float dashDuration = 0.1f; // Duración del dash
    public float dashCooldown = 0.5f; // Tiempo de espera entre dashes
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public Animator animator;

    public float Charactersize = .2f;

    public float dashCooldownTimer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private float dashTime;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        HandleDash();
        AdjustFallSpeed();
    }

    void Move()
    {
        if(isDashing) return;

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(Charactersize, Charactersize, Charactersize);  // Flip character sprite to the right
        else if (moveInput < 0) transform.localScale = new Vector3(-Charactersize, Charactersize, Charactersize); // Flip character sprite to the left
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

    void AdjustFallSpeed()
    {
        if (rb.velocity.y < 0) // When falling
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // When rising but not holding jump
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1f; // Normal gravity
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


    public void Dead(DeathState state)
    {
        this.enabled = false;

        var allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in allEnemies)
            enemy.Stop();  
        

        if (animator != null)
        {
            switch (state)
            {
                case DeathState.Time:
                    animator.SetTrigger("Appear");
                    break;

                case DeathState.Dead:
                    animator.SetTrigger("Appear");
                    break;

                default:
                    Debug.LogWarning("Estado de muerte no reconocido.");
                    break;
            }
        }
    }

}