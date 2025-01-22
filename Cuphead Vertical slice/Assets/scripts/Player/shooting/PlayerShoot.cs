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
    public PlayerMovement PlayerMovement;

    public static bool isShooting = false;

    private Animator animator; // Reference to the Animator component


    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
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
        while (Input.GetMouseButton(0) && PlayerMovement.currentState != PlayerMovement.PlayerState.Jumping)
        {
            Shoot();
            yield return new WaitForSeconds(1f / fireRate);
            //animator.SetBool("IsShooting", true);
        }
        isShooting = false;
        //animator.SetBool("IsShooting", false);
    }

    void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

       
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(spriteRenderer.flipX ? Vector3.left : Vector3.right);
        }
    }
}
