using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject Target;
    public float speed;
    Vector3 differenceVector;
    Vector3 Direction;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        differenceVector = Target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Direction = differenceVector.normalized;
        velocity = Direction * speed * Time.deltaTime;
        transform.position += velocity;

        
    
    }
}
