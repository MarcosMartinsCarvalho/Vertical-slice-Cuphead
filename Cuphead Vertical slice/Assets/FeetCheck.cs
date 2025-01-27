using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class FeetCheck : MonoBehaviour
{

    [SerializeField] public GameObject player;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
         //   player.GetComponent<PlayerMovement>().isGrounded = true;
          //  if (!player.GetComponent<PlayerMovement>().isDashing)
            {
             //   player.GetComponent<PlayerMovement>().currentState = PlayerState.Idle;
            }

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {

            //player.GetComponent<PlayerMovement>().isGrounded = false;
            
        }
    }
}