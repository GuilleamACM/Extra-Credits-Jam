using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public float waitTime = 2f;
    float currentTime = 0f;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.anyKeyDown && currentTime >= waitTime) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }       
    }
}
