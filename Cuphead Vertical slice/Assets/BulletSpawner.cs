using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] GameObject bullet;
    public float cooldown;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown +=  Time.deltaTime;

        if (cooldown > 3) 
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.GetComponent<BossBullet>().Target = Target;
            cooldown = 0;
        }
    }
}
