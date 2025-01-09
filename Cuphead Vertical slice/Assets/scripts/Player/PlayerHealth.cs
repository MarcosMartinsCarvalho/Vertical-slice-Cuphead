using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3; 
    [SerializeField] private float invincibilityDuration = 1.5f; 
    [SerializeField] private SpriteRenderer spriteRenderer; 
    private int currentHP; 
    public bool isInvincible = false;

    private void Start()
    {
        currentHP = maxHP; 
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHP <= 0) return;

        currentHP -= damage;

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


    public int GetCurrentHP()
    {
        return currentHP;
    }
}
