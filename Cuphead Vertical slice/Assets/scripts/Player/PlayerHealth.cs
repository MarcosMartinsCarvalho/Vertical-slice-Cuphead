using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    [SerializeField] private float invincibilityDuration = 1.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public static int currentHP;
    public static bool isInvincible = false;
    private bool TakingDamage = false;
    [SerializeField] private GameObject Health3;
    [SerializeField] private GameObject Health2;
    [SerializeField] private GameObject Health1;
    public static bool isDead = false;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    private void Update()
    {
       
        CheckForAirCollision();
        if (isDead)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.position += new Vector3(0, 1.3f*Time.deltaTime, 0);
            animator.SetTrigger("Floating");
            PlayerMovement.movementSpeed = 0;
            if (transform.position.y > 2.5)
            {
                SceneManager.LoadScene("Defeat");
            }
        }
    }

    private void CheckForAirCollision()
    {
        
        if (transform.position.y <= -5)
        {
            rb.velocity = Vector3.zero;
            if (!isInvincible)
            {

                TakeDamage(1);
            }
            if (!isDead)
            {
                rb.velocity = new Vector2(rb.velocity.x, 24f);
                Debug.Log("Player hit Air and is thrown back up");
            }
            
                
            
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0) return;

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
        isDead = true;
       
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
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.tag == "fireBall" && !isInvincible)
        {
            TakeDamage(1);
        }
    }
}
