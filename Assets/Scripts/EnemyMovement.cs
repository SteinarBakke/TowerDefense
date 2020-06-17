using System.Collections;
using UnityEngine;

//make sure it won't work without Enemy script
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy.ground)
            target = Waypoints.waypoints[0];
        else
            target = WaypointsFlying.waypoints[0];
    }

    void Update()
    {
        if (enemy.dead) return;
        Vector3 dir = target.position - transform.position;
        transform.LookAt(target);

        //Implementing Slow Debuff

        if (enemy.slowed)
            transform.Translate(dir.normalized * (enemy.speed/enemy.slowedAmount) * Time.deltaTime, Space.World);
        else
            transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        //close to waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.3f)
            GetNextWaypoint();

    }

    void GetNextWaypoint()
    {
        if (enemy.ground)
        {
            if (wavepointIndex >= Waypoints.waypoints.Length - 1)
            {
                EndReached();
                return;
            }
        }
        else
        {
            if (wavepointIndex >= WaypointsFlying.waypoints.Length - 1)
            {
                EndReached();
                return;
            }
        }
        wavepointIndex++;
        if (enemy.ground)
            target = Waypoints.waypoints[wavepointIndex];
        else
            target = WaypointsFlying.waypoints[wavepointIndex];
    }

    void EndReached()
    {
        if (!GameManager.gameEnded) //don't play when game is over
            PlayerStats.Health--;
        Destroy(gameObject);
    }
}
