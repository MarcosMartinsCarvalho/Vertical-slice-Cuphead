using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMove : MonoBehaviour
{
    private float x = 1;
    private float xPos = 6.5f;
    private float y;
    public static int health = 0;
    private float bonus = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);
        if (health <= 0)
        {
            bonus -= 1 * Time.deltaTime;
        }
        x += Time.deltaTime;
        y = Mathf.Sin(x)+bonus;

        transform.position = new Vector3(xPos, y, 0);

    }
}
