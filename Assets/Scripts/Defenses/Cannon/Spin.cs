using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cannon))]
public class Spin : MonoBehaviour
{
    Cannon cannon;
    public float speed = 2f;

    private void Awake()
    {
        cannon = GetComponent<Cannon>();
    }
    private void Update()
    {
        foreach (CannonHead c in cannon.GetCannonHeads()) 
        {
            c.transform.RotateAround(transform.position, transform.forward, Time.deltaTime * speed);
        }
    }
}
