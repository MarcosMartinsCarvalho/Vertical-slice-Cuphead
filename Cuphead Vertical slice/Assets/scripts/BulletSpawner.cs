using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject spawnplace;
    [SerializeField] private List<GameObject> platforms = new List<GameObject>();
    [SerializeField] private List<GameObject> allpoints = new List<GameObject>();
    public float cooldown;
    void Start()
    {
        
        
    }

    
    void Update()
    {
        cooldown +=  Time.deltaTime;

        if (cooldown > 3) 
        {
            target = platforms[Random.Range(0, platforms.Count)];
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = spawnplace.GetComponent<Transform>().position;
            newBullet.GetComponent<BossBullet>().Target = target;
            cooldown = 0;
        }
    }
}
