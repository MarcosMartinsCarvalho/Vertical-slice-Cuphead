using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Rigidbody2D rb;
    [Space]
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [Space]
    [SerializeField] private float coyoteTime = 0.08f;

    public enum PlayerState { Idle, MovingLeft, MovingRight, Jumping, Dashing }
    public PlayerState currentState = PlayerState.Idle;

    private readonly HashSet<int> groundColliders = new HashSet<int>();
    private float groundedTimer;
    private bool isDashing;
    private int lastDirection = 1;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (PlayerHealth.isDead)
        {
            currentState = PlayerState.Idle;
            ApplyAnimationState();
            return;
        }

        HandleInput();
        HandleMovement();
        ApplyAnimationState();
    }

    private void FixedUpdate()
    {
        if (groundColliders.Count > 0)
        {
            groundedTimer = coyoteTime;
        }
        else
        {
            groundedTimer -= Time.fixedDeltaTime;
        }

        isGrounded = groundedTimer > 0f;
    }

    private void HandleInput()
    {
        bool movingLeft = Input.GetKey(moveLeft) && !Input.GetKey(moveRight);
        bool movingRight = Input.GetKey(moveRight) && !Input.GetKey(moveLeft);

        if (movingLeft)
        {
            currentState = PlayerState.MovingLeft;
            lastDirection = -1;
        }
        else if (movingRight)
        {
            currentState = PlayerState.MovingRight;
            lastDirection = 1;
        }
        else if (!isDashing && isGrounded)
        {
            currentState = PlayerState.Idle;
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded && !isDashing)
        {
            currentState = PlayerState.Jumping;
        }

        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            currentState = PlayerState.Dashing;
        }

        spriteRenderer.flipX = lastDirection < 0;
    }

    private void HandleMovement()
    {
        if (isDashing)
        {
            return;
        }

        switch (currentState)
        {
            case PlayerState.MovingLeft:
                rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                break;
            case PlayerState.MovingRight:
                rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
                break;
            case PlayerState.Jumping:
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                isGrounded = false;
                currentState = PlayerState.Idle;
                break;
            case PlayerState.Dashing:
                currentState = PlayerState.Idle;
                StartCoroutine(PerformDash());
                break;
            case PlayerState.Idle:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        float dashDirection = lastDirection == 0 ? 1f : lastDirection;
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0);

        yield return new WaitForSeconds(dashLength);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = new Vector2(0, rb.velocity.y);
        isDashing = false;
    }

    private void ApplyAnimationState()
    {
        bool dead = PlayerHealth.isDead;
        bool shooting = PlayerShoot.isShooting && !dead;
        bool groundedForAnimation = isGrounded;
        bool walking = Mathf.Abs(rb.velocity.x) > 0.05f && groundedForAnimation && !isDashing;
        bool jumping = !groundedForAnimation && !isDashing && !dead;
        bool runAndGun = walking && shooting;
        bool idle = groundedForAnimation && !walking && !shooting && !isDashing && !dead;

        animator.SetBool("isGrounded", groundedForAnimation);
        animator.SetBool("IsIdle", idle);
        animator.SetBool("IsWalking", walking);
        animator.SetBool("IsJumping", jumping);
        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("IsShooting", shooting && !runAndGun);
        animator.SetBool("RunAndGun", runAndGun);
        animator.SetBool("isDead", dead);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            return;
        }

        groundColliders.Add(collision.collider.GetInstanceID());
        groundedTimer = coyoteTime;
        isGrounded = true;
        if (!isDashing)
        {
            currentState = PlayerState.Idle;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundColliders.Add(collision.collider.GetInstanceID());
            groundedTimer = coyoteTime;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundColliders.Remove(collision.collider.GetInstanceID());
        }
    }
}
