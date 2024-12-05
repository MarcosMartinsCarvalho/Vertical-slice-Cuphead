using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private Transform bulletSpawnPoint; // Reference to the bullet spawn point
    [SerializeField] private float bulletSpeed = 10f; // Speed of the bullet
    [SerializeField] private float fireRate = 7.5f; // Fire rate in shots per second
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private bool isShooting = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShooting) // Check if the fire button is pressed and not already shooting
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;
        while (Input.GetMouseButton(0)) // Continue shooting while the fire button is held down
        {
            Shoot();
            yield return new WaitForSeconds(1f / fireRate); // Wait for the next shot
        }
        isShooting = false;
    }

    void Shoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Set the bullet's direction based on the SpriteRenderer's flipX property
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(spriteRenderer.flipX ? Vector3.left : Vector3.right);
        }
    }
}
