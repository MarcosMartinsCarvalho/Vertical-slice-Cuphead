using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] healthIcons; // Iconos en la UI

    private void Update()
    {
        int currentHP = playerHealth.GetCurrentHP();

        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].enabled = i < currentHP;
        }
    }
}
