using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int Health = 100;
    [SerializeField] private int DamageToTake = 10;


   
    void takedamage()
    {
        Health = Health - DamageToTake;
        Debug.Log("Boss Health: " + Health);
    }

    private void die()
    {
        Debug.Log("death and taxes");
    }



    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(Health != 0)
            {
                takedamage();
            }
            else
            {
                die();
            }
            
        }
    }
}
