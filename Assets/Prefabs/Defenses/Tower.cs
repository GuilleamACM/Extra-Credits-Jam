using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyGecko.Pathfinding2D;

[RequireComponent(typeof(Structure))]
public abstract class Tower : MonoBehaviour
{
    public bool stalled = true;
    public CannonHead[] cannons;
    public Transform BulletHolder;
    public SpriteRenderer sprite;
    public Material clearMaterial;
    public Material defaultMaterial;

    void OnEnemiesAlive() 
    {
        this.stalled = false;
    }

    void OnNoEnemysAlive() 
    {
        this.stalled = true;
    }

    private void Awake()
    {
        if (!GameManager.Instance.started) { this.stalled = true; }
        GameManager.Instance.StartGameEvent.AddListener(OnEnemiesAlive);
        this.sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        foreach (CannonHead c in cannons)
        {
            c.SetBulletHolder(BulletHolder);
        }
    }

    public void Stall(float TimeInSeconds) 
    {
        sprite.material = clearMaterial;
        Debug.LogWarning("after line!");
        StartCoroutine(StallCoroutine(TimeInSeconds));
    }

    IEnumerator StallCoroutine(float time) 
    {
        this.stalled = true;
        yield return new WaitForSeconds(time);
        sprite.material = defaultMaterial;
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
