using UnityEngine;

public class Platform : MonoBehaviour
{
    private float x = 1;
    [SerializeField] float xPos;
    [SerializeField] float yPos;
    private float y;
    private Vector3 initialPosition;
    private float fallHeight;
    [SerializeField] private bool isFalling = false;
    public bool isDown = false;
    private bool isRising = false;
    private float phaseTimer = 0f;
    [SerializeField] private float fallDistance = 9f;
    [SerializeField] private float fallDuration = 1f;
    [SerializeField] private float downWaitDuration = 4f;
    [SerializeField] private float riseDuration = 0.6f;
    private Animator animator;
    private BoxCollider2D platformCollider;

    void Start()
    {
        initialPosition = transform.position;
        xPos = initialPosition.x;
        yPos = initialPosition.y;
        animator = GetComponent<Animator>();
        platformCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        x += Time.deltaTime;
        y = 0.2f * Mathf.Sin(0.7f * x + 1.57f) + yPos + fallHeight;

        transform.position = new Vector3(xPos, y, 0);

        if (isFalling)
        {
            phaseTimer += Time.deltaTime;
            float t = Mathf.Clamp01(phaseTimer / fallDuration);
            fallHeight = Mathf.Lerp(0f, -fallDistance, t);
            if (t >= 1f)
            {
                isFalling = false;
                isDown = true;
                animator.ResetTrigger("IsFalling");
                phaseTimer = 0f;
            }
        }
        else if (isDown)
        {
            phaseTimer += Time.deltaTime;
            if (phaseTimer >= downWaitDuration)
            {
                isDown = false;
                isRising = true;
                phaseTimer = 0f;
            }
        }
        else if (isRising)
        {
            phaseTimer += Time.deltaTime;
            float t = Mathf.Clamp01(phaseTimer / riseDuration);
            fallHeight = Mathf.Lerp(-fallDistance, 0f, t);
            if (t >= 1f)
            {
                isRising = false;
                fallHeight = 0f;
                phaseTimer = 0f;
                platformCollider.enabled = true;
                animator.SetTrigger("Restored");
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "fireBall" && !isFalling && !isDown && !isRising)
        {
            falling();
            platformCollider.enabled = false;
            animator.SetTrigger("IsFalling");
            animator.ResetTrigger("Restored");
        }
    }

    void falling()
    {
        isFalling = true;
        isDown = false;
        isRising = false;
        phaseTimer = 0f;
    }
}
