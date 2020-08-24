﻿using UnityEngine;
using System.Collections.Generic;
using TinyGecko.Pathfinding2D;

[RequireComponent(typeof(Enemy))]
public class EnemyWaypointMovement : MonoBehaviour
{
    private Queue<GridCel> path;
    private Transform targetTower;
    private Vector3 target;
    private SpriteRenderer spriteRenderer;
    private int currentWaypointIndex = 0;
    bool noTower = false;

    private Enemy enemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        Structure core = Waypoints.CoreSructure;
        path = WorldGrid.Instance.Pathfinder.FindPath(transform.position, core);
        target = path.Dequeue().worldPos;
    }

    private void Update()
    {
        Vector3 waypointDirection = (target - transform.position).normalized * enemy.MovementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target);
        if (distance <= Vector3.Distance(transform.position + waypointDirection,target)) 
        {
            GetNextWaypoint();
        }
        else
        {
            transform.Translate(waypointDirection, Space.World);
        }
    }

    private void GetNextWaypoint()
    {
        if (path.Count == 0)
        {
            ReachObjective();
            return;
        }

        target = path.Dequeue().worldPos;
    }

    private void ReachObjective()
    {
        //Decrease the Player RAM or CPU
        PlayerStatus.Instance.UsedMemory += enemy.memoryUsage;
        WaveSpawner.Instance.RemoveEnemy(enemy);
        Destroy(gameObject);
    }
}