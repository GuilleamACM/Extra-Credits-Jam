using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCannon : MonoBehaviour
{
    public CannonHead cannon;
    public float TimeAiming;
    public GameObject EnemysParent;


    bool targetLocked = false;
    float currentTime = 0;
    [SerializeField]
    Transform target;
    float distance;
    //private List<Enemy> enemys;

    private void Start()
    {
        //this.enemys = GameObject.FindObjectOfType<EnemysManager>().GetEnemys();
    }

    private void Update()
    {
        if (targetLocked && target)
        {
            this.transform.LookAt(target,transform.forward);//verificar isso
            this.currentTime += Time.deltaTime;
            if (this.currentTime >= TimeAiming) 
            {
                Fire();
                ResetTimer();
                this.targetLocked = false;
                this.target = null;
            }
        }
        else 
        {
            TryLockTarget();
        }
    }

    void Fire() 
    {
        cannon.Fire();
    }

    void ResetTimer() 
    {
        this.currentTime = 0;
    }

    void TryLockTarget() 
    {
        //target = null;
        //foreach (Enemy enemy in enemys) 
        //{
        //    float distance = Vector3.Distance(target.transform.position, enemy.transform.position);
        //    if (!target || distance < this.distance) 
        //    {
        //        this.target = enemy.transform;
        //        this.distance = distance;
        //    }
        //}
    }
}
