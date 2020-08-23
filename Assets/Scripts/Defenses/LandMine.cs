using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public static string EnemyTag = "Enemy";
    [Range(0f, 1f)]
    public float slow = .5f;
    public float slowDuration = 2f;
    public float cooldownTime = 4f;
    public float range;
    private List<Transform> enemies;
    bool onCooldown = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

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
            foreach (Transform t in WaveSpawner.Instance.GetEnemiesTransform()) 
            {
                Debug.Log(Vector3.Distance(this.transform.position, t.position));
                if (Vector3.Distance(this.transform.position,t.position) <= range) 
                {
                    t.GetComponent<Enemy>().Slow(slow,slowDuration);
                }
            }
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown () 
    {
        this.onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        this.onCooldown = false;
    }
}
