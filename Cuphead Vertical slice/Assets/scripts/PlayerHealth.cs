using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3; // Vida inicial
    [SerializeField] private float invincibilityDuration = 1.5f; // Duración de invulnerabilidad tras recibir daño
    [SerializeField] private SpriteRenderer spriteRenderer; // Para el parpadeo visual
    private int currentHP; // Vida actual
    public bool isInvincible = false;

    private void Start()
    {
        currentHP = maxHP; // Configura la vida inicial
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHP <= 0) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
        else if (currentHP == 1)
        {
            StartCoroutine(FlashRed()); // Parpadeo al tener 1 HP
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsed = 0;

        while (elapsed < invincibilityDuration)
        {
            // Alternar la visibilidad del sprite para simular parpadeo
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true; // Asegurarse de que el sprite esté visible
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto");
        // Mostrar pantalla de derrota
        // Aquí puedes cargar la escena de derrota
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
}
