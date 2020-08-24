using UnityEngine;
using System.Collections.Generic;
using TinyGecko.Pathfinding2D;

[RequireComponent(typeof(Enemy))]
public class EnemyWaypointMovement : MonoBehaviour
{
    private Queue<GridCel> path;
    private Transform targetTower;
    private Vector3 target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private int currentWaypointIndex = 0;
    bool noTower = false;

    private Enemy enemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
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
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

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
        PlayerStatus.Instance.BlockedMemory += enemy.memoryUsage;
        WaveSpawner.Instance.RemoveEnemy(enemy);
        Destroy(gameObject);
    }
}
