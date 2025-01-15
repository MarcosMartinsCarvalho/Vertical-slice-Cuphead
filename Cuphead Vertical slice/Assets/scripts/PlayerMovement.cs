using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f; // Speed of player movement
    [SerializeField] private float jumpHeight = 5f; // Height of player jump
    [SerializeField] private float dashSpeed = 15f; // Speed of the dash
    [SerializeField] private float dashLength = 0.3f; // Duration of the dash in seconds
    [SerializeField] private bool isGrounded = false; // Tracks if the player is on the ground
    [SerializeField] private Rigidbody2D rb; // Player's Rigidbody2D component
    [SerializeField] private bool canTakeDamage = true; // Determines if the player can take damage

    [SerializeField] private KeyCode moveLeft = KeyCode.A; // Key for moving left
    [SerializeField] private KeyCode moveRight = KeyCode.D; // Key for moving right
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; // Key for jumping
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // Key for dashing

    [SerializeField] private bool IsIdle = false;
    [SerializeField] private bool IsWalking = false;
    [SerializeField] private bool IsJumping = false;
    [SerializeField] private bool IsDashing = false;

    private int getal;
    private int walk = 0;
    private int jump = 0;
    private int shoot = 0;
    private int dash = 0;

    private Animator animator; // Reference to the Animator component


    private enum PlayerState { Idle, MovingLeft, MovingRight, Jumping, Dashing }
    private PlayerState currentState = PlayerState.Idle;

    private bool isDashing = false;
    private int lastDirection = 0;

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
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

        getal = walk + jump + shoot;

        // dash = 8, walk is 4, jump is 2, shoot is 1
    }

    private void HandleInput()
    {
        Debug.Log(getal);

        if (Input.GetKey(moveLeft))
        {
            walk = 4;
            //animator.SetBool("IsIdle", false);

            currentState = PlayerState.MovingLeft;
            lastDirection = -1;

            //animator.SetBool("IsWalking", true);
        }
        else if (Input.GetKey(moveRight))
        {
            walk = 4;
            //animator.SetBool("IsIdle", false);

            currentState = PlayerState.MovingRight;
            lastDirection = 1;

            //animator.SetBool("IsWalking", true);
        }
        else if (isGrounded && !isDashing)
        {
            walk = 0;
            jump = 0;
            //animator.SetBool("IsWalking", false);
            //animator.SetBool("IsJumping", false);
            //animator.SetBool("IsDashing", false);

            currentState = PlayerState.Idle;

            //animator.SetBool("IsIdle", true);
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jump = 2;
            //animator.SetBool("IsIdle", false);

            currentState = PlayerState.Jumping;

            //animator.SetBool("IsJumping", true);
        }

        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            dash = 8;
            //animator.SetBool("IsIdle", false);

            currentState = PlayerState.Dashing;

            //animator.SetBool("IsDashing", true);
        }

        if (PlayerShoot.isShooting)
        {
            shoot = 1;
        }
        else
        {
            shoot = 0;
        }

        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        if (getal == 0)
        {
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
        }
        else if (getal == 4)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
        }
        else if (getal == 2)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
        }
        else if (getal == 8)
        {
            animator.SetBool("IsDashing", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsShooting", false);
        }
        else if (getal == 6)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
        }
        else if (getal == 1)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", true);
        }
        else if (getal == 3) {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", true);
        }
        else if (getal == 5)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", true);
        }


        switch (lastDirection)
        {
            case -1:
                // Flip the sprite renderer horizontally
                GetComponent<SpriteRenderer>().flipX = true;
                break;

            case 1:
                // Flip the sprite renderer horizontally
                GetComponent<SpriteRenderer>().flipX = false;
                break;
        }
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
                    currentState = PlayerState.Idle; // Reset state after jump impulse
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
        canTakeDamage = false;

        // Lock Y axis to prevent falling while dashing
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        // Lock movement and determine dash direction using the last direction
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
        canTakeDamage = true;


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
