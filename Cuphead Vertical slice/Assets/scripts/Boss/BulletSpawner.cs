using System.Collections;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
using System.Collections.Generic;
using System.Threading;
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    public static int health = 75;
    
   
=======
    private Vector3 originalPosition;

>>>>>>> Stashed changes
=======
    private Vector3 originalPosition;

>>>>>>> Stashed changes
    void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
    }

    void Update()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        if (health < 1)
        {
            
            transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            transform.position -= new Vector3(0, 1 * Time.deltaTime, 0);
            cooldown = 0;
        }
        cooldown +=  Time.deltaTime;
        if (cooldown > 0.75) 
=======
        // Handle Idle movement up and down
        if (!isFalling && !isStruggling)
>>>>>>> Stashed changes
=======
        // Handle Idle movement up and down
        if (!isFalling && !isStruggling)
>>>>>>> Stashed changes
        {
            transform.position += new Vector3(0, Mathf.Sin(Time.time * idleMoveSpeed), 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            if (platform.GetComponent<Platform>().isDown == false)
            {
                availible.Add(platform);
                target = availible[Random.Range(0, availible.Count)];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (collision.gameObject.tag == "Bullet")
            {
                health -= 1;
                Destroy(collision.gameObject);
            }
            if (health < 1)
            {
                animator.SetTrigger("Die");
                
            }
        }
    }

=======
            isStruggling = true;
            animator.SetBool("Struggle", true);
        }

        // Check if the platform gets hit by a fireball
        if (collision.gameObject.CompareTag("Fireball"))
        {
            StartCoroutine(FallAndRestore());
        }
    }

=======
            isStruggling = true;
            animator.SetBool("Struggle", true);
        }

        // Check if the platform gets hit by a fireball
        if (collision.gameObject.CompareTag("Fireball"))
        {
            StartCoroutine(FallAndRestore());
        }
    }

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
}
