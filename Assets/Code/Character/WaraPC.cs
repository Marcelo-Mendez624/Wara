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
    public Animator CharAnimator;

    public Color damageColor = Color.red;  // Color del personaje cuando recibe daño
    public Color normalColor = Color.white;  // Color normal del personaje
    public float invulnerabilityDuration = 0.5f;  // Duración de la invulnerabilidad

    private bool isInvulnerable = false;  // Indica si el personaje es invulnerable
    private SpriteRenderer spriteRenderer;
    public int Life = 5;

    public float Charactersize = 1;

    public float dashCooldownTimer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private float dashTime;
    private bool golpeado;


    public Transform SwordPosition;
    public float Radius;
    public LayerMask CollisionLayer;
    public float pushForce = 10f;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        Jump();
        HandleDash();
        AdjustFallSpeed();
        Golpe();
    }

    void Move()
    {
        if(isDashing && golpeado) return;

        
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


        CharAnimator.SetBool("bWalk", moveInput != 0);

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
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
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
            CharAnimator.SetBool("bJump", !isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            CharAnimator.SetBool("bJump", !isGrounded);
        }
    }


    public void Dead(DeathState state)
    {
        this.enabled = false;
        CharAnimator.SetBool("bDead", true);

        var allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in allEnemies)
            enemy.Stop();


        animator.SetTrigger("Appear");
    }

    public void TakeDamage()
    {
        if (isInvulnerable)
            return;  

        Life -= 1;

        spriteRenderer.color = damageColor;

        if (Life <= 0)
        {
            Dead(DeathState.Dead);
            rb.velocity = Vector2.zero;
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        golpeado = true;

        yield return new WaitForSeconds(invulnerabilityDuration);

        spriteRenderer.color = normalColor;
        isInvulnerable = false;
        golpeado = false;
    }


    void Golpe()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CharAnimator.SetTrigger("Attack");

            RaycastHit2D Hit = Physics2D.CircleCast(transform.position, Radius, Vector2.zero, 0, CollisionLayer);


            if (Hit.collider != null)
            {
                // Aplica daño al enemigo
                Hit.collider.GetComponent<Enemy>().TakeDamage();

                // Empuja al enemigo hacia atrás
                Rigidbody2D enemyRb = Hit.collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    // Calcula la dirección de empuje
                    Vector2 pushDirection = (Hit.collider.transform.position - transform.position).normalized;

                    // Aplica la fuerza de empuje
                    enemyRb.AddForce(pushDirection * pushForce);
                }
            }
        }
    }

}