using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canTakeDamage = true;
    [Space]
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [Space]
    [SerializeField] private bool IsIdle = false;
    [SerializeField] private bool IsWalking = false;
    [SerializeField] private bool IsJumping = false;
    [SerializeField] private bool RunAndGun = false;
    [SerializeField] private bool IsDashing = false;
    [Space]
    [SerializeField] private float groundCheckDelay = 0.1f; // Adjustable delay
    private Coroutine groundCheckCoroutine;
    private bool isGroundedDelayed = false; // Stable ground state for logic





    private int getal;
    private int walk = 0;
    private int jump = 0;
    private int shoot = 0;
    private int IsWalkingAndShooting1 = 0;
    private int dash = 0;
  

     private Animator animator; 



    public enum PlayerState { Idle, MovingLeft, MovingRight, Jumping, Dashing }
    public PlayerState currentState = PlayerState.Idle;

    private bool isDashing = false;
    private int lastDirection = 0;

    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    void Update()

    {
        
        if (!isDashing)
        {
            HandleInput();
            PlayerHealth.isInvincible = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            PlayerHealth.isInvincible = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        HandleMovement();

        getal = walk + jump + shoot;

        // dash = 8, walk is 4, jump is 2, shoot is 1
        UpdateGroundedState();
    }
    private void UpdateGroundedState()
    {
        if (isGrounded != isGroundedDelayed)
        {
            if (groundCheckCoroutine != null)
            {
                StopCoroutine(groundCheckCoroutine);
            }
            groundCheckCoroutine = StartCoroutine(GroundCheckDelayCoroutine());
        }
    }
    private IEnumerator GroundCheckDelayCoroutine()
    {
        yield return new WaitForSeconds(groundCheckDelay);
        isGroundedDelayed = isGrounded; // Apply stable ground state after delay
        animator.SetBool("isGrounded", isGroundedDelayed);
    }
    private void HandleInput()
    {
        //Debug.Log(getal);
        
        if (Input.GetKey(moveLeft) && !PlayerHealth.isDead)
        {
            walk = 4;
            //animator.SetBool("IsIdle", false);

            currentState = PlayerState.MovingLeft;
            lastDirection = -1;
            movementSpeed = 5f;

            //animator.SetBool("IsWalking", true);
        }
        else if (Input.GetKey(moveRight) && !PlayerHealth.isDead)
        {
            walk = 4;
            //animator.SetBool("IsIdle", false);

            currentState = PlayerState.MovingRight;
            lastDirection = 1;
            movementSpeed = 5f;

            //animator.SetBool("IsWalking", true);
        }
        else if(!Input.GetKey(moveLeft) && !Input.GetKey(moveRight))
        {
            movementSpeed = 0f;
            
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

        if (Input.GetKeyDown(dashKey) && !isDashing && !PlayerHealth.isDead)
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

        if (PlayerHealth.isDead)
        {
            walk = -1;
            jump = 0;
            shoot = 0;
            dash = 0;
        }

        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
        if (getal == 7)
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", true);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }

        if (getal == 0)
        {
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 4)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 2)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 8)
        {
            animator.SetBool("IsDashing", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 6)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 1)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", true);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 3)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", true);
            animator.SetBool("RunAndGun", false);
            animator.SetBool("isDead", false);
        }
        else if (getal == 5)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", true);
            animator.SetBool("isDead", false);

        }
        else if(getal == -1) {
            animator.SetBool("isDead", true);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDashing", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("RunAndGun", false);
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
                    IsIdle = true;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    break;
            }
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canTakeDamage = false;

       
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        
        float dashDirection = lastDirection;
        if (dashDirection != 0)
        {
            rb.velocity = new Vector2(dashDirection * dashSpeed, 0);
        }

        yield return new WaitForSeconds(dashLength);

        
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
        
        if (collision.gameObject.CompareTag("Ground"))
        {

            isGrounded = false;
        }
    }


}