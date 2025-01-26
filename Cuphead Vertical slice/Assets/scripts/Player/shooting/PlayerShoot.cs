using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform bulletSpawnPoint; 
    [SerializeField] private float bulletSpeed = 10f; 
    [SerializeField] private float fireRate = 7.5f; 
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space]


    public static bool isShooting = false;

    [SerializeField] Animator ParticleAnimation; // Reference to the Animator component


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            StartCoroutine(ShootCoroutine());
        }
        
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;
        while (Input.GetMouseButton(0) && gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            Shoot();
            yield return new WaitForSeconds(1f / fireRate);
        }
        isShooting = false;
    }

    void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        ParticleAnimation.SetTrigger("Shot");
       
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(spriteRenderer.flipX ? Vector3.left : Vector3.right);
        }
    }
}
