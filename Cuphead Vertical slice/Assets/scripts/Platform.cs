using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    public float sinkSpeed = 1f; 
    public float resetSpeed = 1f; 
    public float sinkDelay = 1f; 
    public Transform groundCheck; 
    public LayerMask playerLayer; 
    public float groundCheckRadius = 0.5f;
    private float x = 1;
    [SerializeField] float xPos;
    [SerializeField] float yPos;
    private float y;
    private Vector3 initialPosition; 
    private bool isSinking = false; 
    private bool isReturning = false;
    public float fallTimer = 0;
    private float fallHeight;
    private bool isFalling = false;
    public bool isDown = false;
    public float waitTimer = 0f;
    public float riseTimer = 0f;

    void Start()
    {
        initialPosition = transform.position;
        xPos = initialPosition.x;
        yPos = initialPosition.y;
    }

    void Update()
    {

      //  CheckPlayerOnPlatform();
        x += Time.deltaTime;
        y = 0.2f * Mathf.Sin(0.7f*x + 1.57f) + yPos + fallHeight;

        transform.position = new Vector3(xPos, y, 0);
        if(isFalling == true)
        {
            
            fallHeight -= 9* Time.deltaTime;
            fallTimer += Time.deltaTime;
            if(fallTimer > 1)
            {
                isFalling = false;
                isDown = true;
                           
            }
        }
        if (isDown == true)
        {
            
            waitTimer += Time.deltaTime;
            if (waitTimer > 4)
            {
                fallHeight += 9 * Time.deltaTime;
                riseTimer += Time.deltaTime;
            }
            
        }
        if (riseTimer > 1)
        {
            isDown = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    
    void OnTriggerEnter2D(Collider2D col)
    {
       

        if (col.gameObject.tag == "fireBall")
        {
           
            falling();  
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } 
    }
    void falling()
    {
        isFalling = true;
        riseTimer = 0;
        waitTimer = 0;
        fallTimer = 0;
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
