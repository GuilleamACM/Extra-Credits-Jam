using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHead : MonoBehaviour
{
    public GameObject BulletPrefab;
    Transform bulletHolder;

    public void SetBulletHolder(Transform bulletHolder) 
    {
        this.bulletHolder = bulletHolder;
    }

    public void Fire()
    {
        var bullet = Instantiate(BulletPrefab, bulletHolder);
        bullet.transform.position = transform.position;
        bullet.transform.localScale = Vector3.one;
        bullet.GetComponent<Bullet>().Direction = this.transform.right;
    }
    public void Fire(Vector3 dir)
    {
        var bullet = Instantiate(BulletPrefab, bulletHolder);
        bullet.transform.position = transform.position;
        bullet.transform.localScale = Vector3.one;
        bullet.GetComponent<Bullet>().Direction = dir;
    }
}
