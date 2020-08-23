using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Tower
{
    public float TimeBetweenShots = 2f;
    float currentTime = 0f;

    public CannonHead[] GetCannonHeads() => base.cannons;

    private void Update()
    {
        this.currentTime += Time.deltaTime;
        if (currentTime >= TimeBetweenShots) 
        {
            base.Fire();
            ResetTimer();
        }
    }

    void ResetTimer() 
    {
        this.currentTime = 0f;
    }
}
