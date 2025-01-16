using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canTakeDamage = true;

    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode shootKey = KeyCode.F;

    private int lastDirection = 0;
    private bool isDashing = false;
    private PlayerState currentState = PlayerState.Idle;
    private Animator animator;

    private enum PlayerState
    {
        Idle,
        MovingLeft,
        MovingRight,
        Jumping,
        Dashing,
        Shooting,
        RunningAndShooting
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDashing)
        {
            HandleInput();
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        HandleMovement();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        bool isMoving = Input.GetKey(moveLeft) || Input.GetKey(moveRight);
        bool isShooting = Input.GetKey(shootKey);

        if (isMoving && isShooting)
        {
            currentState = PlayerState.RunningAndShooting;
        }
        else if (Input.GetKey(moveLeft) && !Input.GetKey(moveRight))
        {
            currentState = PlayerState.MovingLeft;
            lastDirection = -1;
        }
        else if (Input.GetKey(moveRight) && !Input.GetKey(moveLeft))
        {
            currentState = PlayerState.MovingRight;
            lastDirection = 1;
        }
        else if (isShooting)
        {
            currentState = PlayerState.Shooting;
        }
        else if (isGrounded && !isDashing)
        {
            currentState = PlayerState.Idle;
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            currentState = PlayerState.Jumping;
        }

        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            currentState = PlayerState.Dashing;
        }

        GetComponent<SpriteRenderer>().flipX = lastDirection == -1;
    }

    private void HandleMovement()
    {
        if (isDashing) return;

        switch (currentState)
        {
            case PlayerState.MovingLeft:
            case PlayerState.RunningAndShooting:
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
                StartCoroutine(PerformDash());
                break;

            case PlayerState.Idle:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;

            case PlayerState.Shooting:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canTakeDamage = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        rb.velocity = new Vector2(lastDirection * dashSpeed, 0);
        yield return new WaitForSeconds(dashLength);

        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDashing = false;
        canTakeDamage = true;

        if (isGrounded) currentState = PlayerState.Idle;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsIdle", currentState == PlayerState.Idle);
        animator.SetBool("IsWalking", currentState == PlayerState.MovingLeft || currentState == PlayerState.MovingRight);
        animator.SetBool("IsJumping", currentState == PlayerState.Jumping);
        animator.SetBool("IsDashing", currentState == PlayerState.Dashing);
        animator.SetBool("IsShooting", currentState == PlayerState.Shooting);
        animator.SetBool("IsRunningAndShooting", currentState == PlayerState.RunningAndShooting);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
