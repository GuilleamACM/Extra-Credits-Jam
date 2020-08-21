using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHead : MonoBehaviour
{
    public GameObject BulletPrefab;

    private void Start()
    {
        Fire();
    }

    public void Fire()
    {
        var bullet = Instantiate(BulletPrefab,transform);
        bullet.transform.position = Vector3.zero;
        bullet.GetComponent<Bullet>().Direction = this.transform.forward;
    }
}
