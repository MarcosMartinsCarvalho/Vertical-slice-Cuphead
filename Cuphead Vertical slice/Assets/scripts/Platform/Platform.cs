using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public enum PlatformState
    {
        Idle,
        Falling,
        Struggling
    }

    [SerializeField] private PlatformState currentState;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float struggleDip = 0.5f;
    [SerializeField] private float fallTime = 2f;
    [SerializeField] private float waitTimeAfterStruggling = 2f; 
    private Vector3 initialPosition;
    [SerializeField] private bool isPlayerOnPlatform;
    private bool isFalling;
    [SerializeField] private bool isStruggling;

    public bool isDown = false;

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(PlatformStates());
    }

    private IEnumerator PlatformStates()
    {
        while (true)
        {
            switch (currentState)
            {
                case PlatformState.Idle:
                    float moveTime = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
                    transform.position = initialPosition + Vector3.up * moveTime;
                    yield return null;
                    break;

                case PlatformState.Falling:
                    if (!isFalling)
                    {
                        isFalling = true;
                        isDown = true; 
                        float timer = 0f;
                        Vector3 fallPosition = transform.position - Vector3.up * 5f; 

                        while (timer < fallTime)
                        {
                            transform.position = Vector3.Lerp(transform.position, fallPosition, timer / fallTime);
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        transform.position = fallPosition;
                        yield return new WaitForSeconds(fallTime); 

                        timer = 0f;
                        while (timer < fallTime)
                        {
                            transform.position = Vector3.Lerp(transform.position, initialPosition, timer / fallTime);
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        transform.position = initialPosition;
                        isDown = false; 
                        isFalling = false;
                    }
                    yield return null;
                    break;

                case PlatformState.Struggling:
                    
                    
                    yield return null;
                    break;
            }
        }
    }

    public void ChangeState(PlatformState newState)
    {
        currentState = newState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            isStruggling = true;
            transform.position = new Vector3(transform.position.x, transform.position.y - struggleDip, transform.position.z);
        }

        if (collision.gameObject.CompareTag("fireBall"))
        {
            currentState = PlatformState.Falling;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }
}
