using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : Tower
{
    public static string EnemyTag = "Enemy";
    [Range(0f, 1f)]
    public float slow = .5f;
    public float slowDuration = 2f;
    public float cooldownTime = 4f;
    public float range;
    public int maxTargetsBeforeCooldown = 3;
    private List<Transform> enemies;
    bool onCooldown = false;
    private int affected = 0;

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
        if(!stalled && !onCooldown)
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
                    affected++;
                    t.GetComponent<Enemy>().Slow(slow,slowDuration);
                }
            }
            if(affected == maxTargetsBeforeCooldown)
                StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown () 
    {
        this.onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        affected = 0;
        this.onCooldown = false;
    }
}
