using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float fallSpeed = -20f; // Speed of falling
    [SerializeField] private float idleMoveSpeed = 0.1f; // Speed of idle up-and-down movement
    [SerializeField] private float idleMovementRange = 2f; // Range of idle up-and-down movement
    [SerializeField] private float struggleDownAmount = 0.5f; // How far down the platform moves while struggling
    [SerializeField] private float restoreSpeed = 5f; // Speed of restoring after falling
    [SerializeField] private KeyCode testFallKey = KeyCode.F; // Key to manually test the fall mechanic
    [SerializeField] private GameObject platform;

    private bool isFalling = false;
    private bool isStruggling = false;
    private bool isDown = false; // Tracks if the platform is currently falling
    private Animator animator;
    private Vector3 originalPosition;
    private float localTimeOffset = 0f; // Keeps track of sine wave progress for idle movement
    private GameObject playerOnPlatform = null; // Tracks the player object standing on the platform

    void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
    }


    void Update()
    {
        // Check for the test fall key
        if (Input.GetKeyDown(testFallKey))
        {
            StartCoroutine(FallAndRestore());
        }

        // Handle platform behavior
        if (isFalling) return; // Don't move if falling

        if (isStruggling)
        {
            // Move slightly down while struggling (only once, avoid snapping)
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(originalPosition.x, originalPosition.y - struggleDownAmount, originalPosition.z),
                restoreSpeed * Time.deltaTime);

            // Adjust the player's position to match the platform
            if (playerOnPlatform != null)
            {
                playerOnPlatform.transform.position += Vector3.down * (struggleDownAmount * Time.deltaTime);
            }
        }
        else
        {
            // Handle idle up-and-down movement
            localTimeOffset += Time.deltaTime;
            float sinValue = Mathf.Sin(localTimeOffset * idleMoveSpeed) * idleMovementRange; // Oscillation
            transform.position = new Vector3(originalPosition.x, originalPosition.y + sinValue, originalPosition.z);

            // Adjust the player's position to stay grounded on the platform
            if (playerOnPlatform != null)
            {
                playerOnPlatform.transform.position += new Vector3(0, sinValue * Time.deltaTime, 0);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            isStruggling = true;
            animator.SetBool("Struggle", true);
            playerOnPlatform = collision.gameObject; // Store the reference to the player
        }

        // Check if the platform gets hit by a fireball
        if (collision.gameObject.CompareTag("fireBall"))
        {
            StartCoroutine(FallAndRestore());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Stop struggling when the player leaves the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            isStruggling = false;
            animator.SetBool("Struggle", false);
            playerOnPlatform = null; // Clear the reference to the player
        }
    }

    private IEnumerator FallAndRestore()
    {
        // Start falling
        isDown = true; // Mark platform as falling
        isFalling = true;
        animator.SetBool("IsFalling", true);
        float fallStartTime = Time.time;

        // Fall for 4 seconds
        while (Time.time - fallStartTime < 4f)
        {
            transform.position += new Vector3(0, fallSpeed * Time.deltaTime, 0);
            if (playerOnPlatform != null)
            {
                playerOnPlatform.transform.position += new Vector3(0, fallSpeed * Time.deltaTime, 0); // Move player along
            }
            yield return null;
        }

        // Smoothly restore to original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, restoreSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the platform ends at the exact original position
        transform.position = originalPosition;

        // Reset states
        localTimeOffset = 0f; // Reset idle sine wave calculation to avoid jerking
        isDown = false; // Mark platform as no longer falling
        isFalling = false;
        animator.SetBool("IsFalling", false);
    }

    // Public property to check if the platform is down
    public bool IsDown => isDown;
}
