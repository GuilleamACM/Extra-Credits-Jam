using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCannon : MonoBehaviour
{
    public CannonHead cannon;
    public float TimeAiming;


    float currentTime = 0;
    [SerializeField]
    Transform target;
    float distance;
    private List<Transform> enemys;

    private void Start()
    {
        this.enemys = WaveSpawner.Instance.GetEnemys();
    }

    private void Update()
    {
        if (target)
        {
            LookAtTarget();
            this.currentTime += Time.deltaTime;
            if (this.currentTime >= TimeAiming) 
            {
                Fire();
                ResetTimer();
                this.target = null;
            }
        }
        else 
        {
            TryLockTarget();
        }
    }

    public float turnSpeed = 20f;

    void LookAtTarget() 
    {
        //float dist = 0.15f;
        //Vector3 targetDir = target.position - transform.position;
        //
        //float angle = Vector3.Angle(targetDir, Vector3.right);
        //
        //angle *= Mathf.Deg2Rad;
        //angle += Mathf.PI / 2;
        //Vector3 right = new Vector3(0, dist, 0);
        //if (targetDir.y > 0) 
        //{
        //    right *= -1;
        //}
        //
        //cannon.transform.localPosition = right * Mathf.Cos(angle) + new Vector3(dist,0,0) * Mathf.Sin(angle);

        Vector3 targetDir = target.position - transform.position;
        Quaternion LookRotation = Quaternion.LookRotation(targetDir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, LookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        Debug.Log(rotation.z);
        transform.rotation = Quaternion.Euler(0f,0f,rotation.z);
        if (targetDir.x < 0f)
        {
            transform.Rotate(0,0,180f);
        }


        //Debug.Log(cannon.transform.localPosition.normalized);


        //transform.eulerAngles = new Vector3(0,0,Mathf.Rad2Deg * angle);
        //transform.LookAt(target.position);
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
        target = null;
        foreach (Transform enemy in enemys) 
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (!target || distance < this.distance) 
            {
                this.target = enemy.transform;
                this.distance = distance;
            }
        }
    }
}
