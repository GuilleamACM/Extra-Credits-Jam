using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 5.0f;
    public Vector3 Direction;
    public int Damage = 10;
    public float LifeTime = 10f;
    float currentLifeTime = 0f;
    public BulletType type;

    public enum BulletType 
    {
        Normal,
        Pierce
    }

    public static string EnemyTag = "Enemy";

    private void Start()
    {
        this.Direction = this.Direction.normalized;
    }

    void Update()
    {
        Move();
        UpdateTime();
        if (currentLifeTime > LifeTime) 
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateTime() 
    {
        this.currentLifeTime += Time.deltaTime;
    }

    private void Move()
    {
        transform.position += Direction * Time.deltaTime * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitCheck(collision);   
    }

    void HitCheck(Collider2D collision) 
    {
        if (collision.CompareTag(EnemyTag))
        {
            collision.GetComponent<Enemy>().TakeDamage(this.Damage);
            if(this.type == BulletType.Normal)
                Destroy(this.gameObject);
        }
    }
}
