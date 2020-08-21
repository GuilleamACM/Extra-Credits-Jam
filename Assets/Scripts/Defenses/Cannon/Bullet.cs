using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 5.0f;
    public Vector3 Direction;
    public int Damage = 10;

    private void Start()
    {
        this.Direction = this.Direction.normalized;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += Direction * Time.deltaTime * Speed;
    }
}
