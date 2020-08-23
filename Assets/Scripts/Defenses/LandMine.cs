using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public static string EnemyTag = "Enemy";
    public int Damage = 0;
    [Range(0f, 1f)]
    public float slow = .5f;
    public float time = 2f;
    private List<Transform> enemies;

    private void Start()
    {
        this.enemies = WaveSpawner.Instance.GetEnemiesTransform();
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
            Destroy(this.gameObject);
        }
    }

}
