using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f; 
    [SerializeField] private float dashSpeed = 15f; 
    [SerializeField] private float dashLength = 0.3f; 
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Rigidbody2D rb;
    public Transform groundCheck; 
    public LayerMask groundLayer; 
    public float groundCheckRadius = 0.2f; 
    private PlayerHealth playerHealth;

    [SerializeField] private KeyCode moveLeft = KeyCode.A; 
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; 
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; 
    private enum PlayerState { Idle, MovingLeft, MovingRight, Jumping, Dashing }
    private PlayerState currentState = PlayerState.Idle;

    private bool isDashing = false;
    private int lastDirection = 0;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if (!isDashing)
        {
            HandleInput();
        }

        HandleMovement();
        CheckGround();
    }

    private void HandleInput()
    {
        if (Input.GetKey(moveLeft))
        {
            currentState = PlayerState.MovingLeft;
            lastDirection = -1;
        }
        else if (Input.GetKey(moveRight))
        {
            currentState = PlayerState.MovingRight;
            lastDirection = 1;
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

        
        switch (lastDirection)
        {
            case -1:
       
                GetComponent<SpriteRenderer>().flipX = true;
                break;

            case 1:
               
                GetComponent<SpriteRenderer>().flipX = false;
                break;
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleMovement()
    {
        if (!isDashing)
        {
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
                    StartCoroutine(PerformDash());
                    break;

                case PlayerState.Idle:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    break;
            }
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        playerHealth.isInvincible = false;

        
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        float dashDirection = lastDirection;
        if (dashDirection != 0)
        {
            rb.velocity = new Vector2(dashDirection * dashSpeed, 0);
        }

        yield return new WaitForSeconds(dashLength);

        // Stop movement and reset constraints
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        isDashing = false;
        playerHealth.isInvincible = true;

        
        if (isGrounded)
        {
            currentState = PlayerState.Idle;
        }
        else if (Input.GetKey(moveLeft))
        {
            currentState = PlayerState.MovingLeft;
        }
        else if (Input.GetKey(moveRight))
        {
            currentState = PlayerState.MovingRight;
        }
        else
        {
            currentState = PlayerState.Idle;
        }
    }
    
    
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // groundcheck
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (!isDashing)
            {
                currentState = PlayerState.Idle;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        // groundcheck
        if (collision.gameObject.CompareTag("Ground"))
        {
        
            isGrounded = false;
        }
    }
}
