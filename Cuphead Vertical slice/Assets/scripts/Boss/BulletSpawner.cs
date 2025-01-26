using System.Collections;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float fallSpeed = -20f;
    [SerializeField] private float idleMoveSpeed = 0.1f;
    [SerializeField] private GameObject platform;

    private bool isFalling = false;
    private bool isStruggling = false;
    private bool isDown = false;  // isDown added to track if the platform is falling
    private Animator animator;
    private Vector3 originalPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
    }

    void Update()
    {
        // Handle Idle movement up and down
        if (!isFalling && !isStruggling)
        {
            transform.position += new Vector3(0, Mathf.Sin(Time.time * idleMoveSpeed), 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            isStruggling = true;
            animator.SetBool("Struggle", true);
        }

        // Check if the platform gets hit by a fireball
        if (collision.gameObject.CompareTag("Fireball"))
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
        }
    }

    private IEnumerator FallAndRestore()
    {
        // Start falling
        isDown = true;  // Set isDown to true when falling
        isFalling = true;
        animator.SetBool("IsFalling", true);
        float fallStartTime = Time.time;

        // Fall for 4 seconds
        while (Time.time - fallStartTime < 4f)
        {
            transform.position += new Vector3(0, fallSpeed * Time.deltaTime, 0);
            yield return null;
        }

        // Restore to original position after 4 seconds
        transform.position = originalPosition;
        isDown = false;  // Set isDown to false when restoring
        isFalling = false;
        animator.SetBool("IsFalling", false);
    }

    // Public property to access isDown
    public bool IsDown => isDown;
}
