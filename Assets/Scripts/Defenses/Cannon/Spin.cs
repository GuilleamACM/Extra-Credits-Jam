using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 2f;
    private void Update()
    {
        transform.Rotate(transform.forward, Time.deltaTime * speed);
    }
}
