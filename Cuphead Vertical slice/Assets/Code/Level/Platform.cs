using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    public float sinkSpeed = 1f; 
    public float resetSpeed = 1f; 
    public float sinkDelay = 1f; 
    public Transform groundCheck; 
    public LayerMask playerLayer; 
    public float groundCheckRadius = 0.5f;

    private Vector3 initialPosition; 
    private bool isSinking = false; 
    private bool isReturning = false; 

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        CheckPlayerOnPlatform();
    }

    void CheckPlayerOnPlatform()
    {

        bool playerOnPlatform = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, playerLayer);

        if (playerOnPlatform && !isSinking && !isReturning)
        {
            StartCoroutine(SinkPlatform());
        }
    }

    IEnumerator SinkPlatform()
    {
        isSinking = true;

        yield return new WaitForSeconds(sinkDelay);


        while (transform.position.y > initialPosition.y - 2f) 
        {

            transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
            yield return null;
        }



        yield return new WaitForSeconds(1f);


        StartCoroutine(ReturnToInitialPosition());
    }

    IEnumerator ReturnToInitialPosition()
    {
        isReturning = true;

        while (transform.position.y < initialPosition.y)
        {
            transform.position += new Vector3(0, resetSpeed * Time.deltaTime, 0);
            yield return null;
        }

        transform.position = initialPosition;
        isSinking = false;
        isReturning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
