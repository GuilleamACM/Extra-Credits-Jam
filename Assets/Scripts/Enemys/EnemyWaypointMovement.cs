using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyWaypointMovement : MonoBehaviour
{
    private Transform target;
    private int currentWaypointIndex = 0;

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.waypoints[0];
    }

    private void Update()
    {
        Vector3 waypointDirection = target.position - transform.position;
        transform.Translate(waypointDirection.normalized * enemy.MovementSpeed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.05f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if(currentWaypointIndex >= Waypoints.waypoints.Length - 1)
        {
            ReachObjective();
            return;
        }

        currentWaypointIndex++;
        target = Waypoints.waypoints[currentWaypointIndex];
    }

    private void ReachObjective()
    {
        //Decrease the Player RAM or CPU
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}
