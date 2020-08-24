using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HijackerBullet : MonoBehaviour
{
    public readonly string towerTag = "Tower";
    public readonly string wallTag = "Wall";

    public GameObject hitFX;
    public Vector3 direction;
    public float speed = 3f;
    public float hijackTime = 1f;


    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitCheck(collision);
    }

    void HitCheck(Collider2D collision)
    {
        if (collision.CompareTag(towerTag))
        {
            var fx = Instantiate(hitFX);
            fx.transform.position = transform.position;
            collision.GetComponent<Tower>().Stall(hijackTime);
            Destroy(this.gameObject);
        }
        if (collision.CompareTag(wallTag))
        {
            Destroy(this.gameObject);
        }
    }
}
