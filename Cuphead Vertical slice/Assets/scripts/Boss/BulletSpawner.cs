using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject spawnplace;
    [SerializeField] private List<GameObject> availible = new List<GameObject>();
    [SerializeField] private List<GameObject> allplatforms = new List<GameObject>();
    public float cooldown;
    void Start()
    {
        
        
    }

    
    void Update()
    {
        cooldown +=  Time.deltaTime;

        if (cooldown > 3) 
        {
            availible.Clear();
            CheckAvailible();    
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = spawnplace.GetComponent<Transform>().position;
            newBullet.GetComponent<BossBullet>().Target = target;
            cooldown = 0;
        }
    }

    void CheckAvailible()
    {
        
        foreach (GameObject platform in allplatforms)
        {
            if (platform.GetComponent<Platform>().isDown == false)
            {
                availible.Add(platform);
                target = availible[Random.Range(0, availible.Count)];
            }
        }
    }
}
