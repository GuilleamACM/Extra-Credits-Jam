using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyGecko.Pathfinding2D;

[RequireComponent(typeof(Structure))]
public abstract class Tower : MonoBehaviour
{
    [SerializeField]
    bool stalled = false;
    public CannonHead[] cannons;
    public Transform BulletHolder;

    private void Start()
    {
        foreach (CannonHead c in cannons)
        {
            c.SetBulletHolder(BulletHolder);
        }
    }

    public void Stall(float TimeInSeconds) 
    {
        StartCoroutine(StallCoroutine(TimeInSeconds));
    }

    IEnumerator StallCoroutine(float time) 
    {
        this.stalled = true;
        yield return new WaitForSeconds(time);
        this.stalled = false;
    }

    protected virtual bool Fire() 
    {
        if(!stalled)
            FireAllCannons();
        return stalled;
    }

    void FireAllCannons() 
    {
        foreach (CannonHead c in this.cannons)
            c.Fire();
    }

}
