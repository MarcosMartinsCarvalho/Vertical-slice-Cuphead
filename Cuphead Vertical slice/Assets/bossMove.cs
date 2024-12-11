using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMove : MonoBehaviour
{
    private float x = 1;
    private float xpos = 6.5f;
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime;
        y = Mathf.Sin(x);

        transform.position = new Vector3(xpos, y, 0);
    }
}
