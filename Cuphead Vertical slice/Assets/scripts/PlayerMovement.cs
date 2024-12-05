using System.Collections;
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

    private enum PlayerState { Idle, MovingLeft, MovingRight, Jumping, Dashing }
    private PlayerState currentState = PlayerState.Idle;

    private bool isDashing = false;
    private int lastDirection = 0;

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
        else if (!isDashing)
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
                    rb.velocity = new Vector2(movementSpeed, jumpHeight);
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
        rb.velocity = new Vector2(0, rb.velocity.y); // Keep the current vertical velocity (e.g., gravity effect)
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
        // Check if the collided object has the tag "Ground"
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
        // If the player leaves a collision with an object tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
