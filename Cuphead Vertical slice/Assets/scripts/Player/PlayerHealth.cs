using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3; 
    [SerializeField] private float invincibilityDuration = 1.5f; 
    [SerializeField] private SpriteRenderer spriteRenderer; 
    private int currentHP; 
    public bool isInvincible = false;
    private bool TakingDamage = false;
    public GameObject Health3;
    public GameObject Health2;
    public GameObject Health1;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHP <= 0) return;

        currentHP -= damage;
        Debug.Log("Player HP: " + currentHP);
        //animator.SetBool("Pain", true);
        StartCoroutine(PainAnim());


        if (currentHP <= 0)
        {
            Die();
        }
        

        StartCoroutine(InvincibilityCoroutine());

        
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsed = 0;

        while (elapsed < invincibilityDuration)
        {
            
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true; 
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto");

    }

    private IEnumerator PainAnim()
    {
        if (TakingDamage) 
        {
            
        } 
        else
        {
            TakingDamage = true;
            animator.SetBool("Pain", true);
            UpdateHealthUI();
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Pain", false);
            TakingDamage = false;
        }
        
    }

    private void UpdateHealthUI()
    {
        if (currentHP == 3)
        {
            Health3.SetActive(true);
            Health2.SetActive(false);
            Health1.SetActive(false);
        }
        else if (currentHP == 2)
        {
            Health3.SetActive(false);
            Health2.SetActive(true);
            Health1.SetActive(false);
        }
        else if (currentHP == 1)
        {
            Health3.SetActive(false);
            Health2.SetActive(false);
            Health1.SetActive(true);
        }
        else if (currentHP == 0)
        {
            Health3.SetActive(false);
            Health2.SetActive(false);
            Health1.SetActive(false);
        }
    }



    public int GetCurrentHP()
    {
        return currentHP;
    }
}
