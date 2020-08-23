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
        Vector3 waypointDirection = (target.position - transform.position).normalized * enemy.MovementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= Vector3.Distance(transform.position + waypointDirection,target.position)) 
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
        PlayerStatus.Instance.UsedMemory += enemy.memoryUsage;
        WaveSpawner.Instance.RemoveEnemy(enemy);
        Destroy(gameObject);
    }
}
