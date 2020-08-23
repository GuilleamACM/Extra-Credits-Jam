using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        bullet.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
        bullet.GetComponent<Bullet>().Direction = this.transform.right;
    }
    public void Fire(Vector3 dir)
    {
        var bullet = Instantiate(BulletPrefab, bulletHolder);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Bullet>().Direction = dir;
    }
}
