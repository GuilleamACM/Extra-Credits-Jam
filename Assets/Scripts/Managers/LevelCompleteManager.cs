using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteManager : MonoBehaviour
{
    public float waitTime = 2f;
    float currentTime = 0f;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.anyKeyDown && currentTime >= waitTime)
        {
            if (LevelHolder.Instance.SetCurrentLevelAsNextLevel())
            {
                LevelHolder.Instance.LoadCurrentLevel();
            }
            else 
            {
                Debug.Log("That's all folks!");
            }
        }
    }
}
