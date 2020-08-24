using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HijackerCannon : MonoBehaviour
{
    public GameObject HijackerBullet;
    public float timeBetweenShots = 1f;
    float currentTime;

    public Vector3[] fireDirections;
    public int directionIndex;


    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeBetweenShots) 
        {
            Fire();
            ResetTimer();
        }
    }

    void ResetTimer() 
    {
        currentTime = 0f;
    }

    void Fire() 
    {
        UpdateDirection();
        Instantiate(HijackerBullet);
        HijackerBullet.transform.position = this.transform.position;
        HijackerBullet.GetComponent<HijackerBullet>().direction = fireDirections[directionIndex];
    }
    void UpdateDirection() 
    {
        directionIndex = (directionIndex + 1) % fireDirections.Length;
    }

}
