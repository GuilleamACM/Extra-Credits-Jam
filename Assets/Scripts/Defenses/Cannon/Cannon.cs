using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public CannonHead[] Cannons;
    public Transform BulletHolder;

    public float TimeBetweenShots = 2f;
    float currentTime = 0f;

    private void Start()
    {
        foreach (CannonHead c in this.Cannons) 
        {
            c.SetBulletHolder(BulletHolder);
        }
    }

    private void Update()
    {
        this.currentTime += Time.deltaTime;
        if (currentTime >= TimeBetweenShots) 
        {
            Fire();
            ResetTimer();
        }
    }

    void ResetTimer() 
    {
        this.currentTime = 0f;
    }

    void Fire() 
    {
        foreach (CannonHead c in this.Cannons) 
        {
            c.Fire();
        }
    }
}
