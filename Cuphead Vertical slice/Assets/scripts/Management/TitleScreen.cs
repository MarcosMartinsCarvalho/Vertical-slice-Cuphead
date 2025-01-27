using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenSwapper : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown)
        {
            BulletSpawner.deadMothTimer = 0;
            BulletSpawner.health = 75;
            PlayerHealth.currentHP = 3;
            PlayerHealth.isDead = false;
            SceneManager.LoadScene("FinalGame");
        }
    }
}
