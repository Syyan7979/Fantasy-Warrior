using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPathing : MonoBehaviour
{
    // Config Params
    [SerializeField] float movementSpeed = 4.5f;
    [SerializeField] float enemyPauseTime = 1.2f;

    // Dynamic Variables
    List<Transform> waypoints;
    int waypointIndex = 0;
    Animator animator;
    Vector3 oldPos;
    bool enemyPause = false;
    bool start = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        Increment();
    }

    // solves for the increments done on our enemy which is used for its walking animation
    private void Increment()
    {
        float diff = (transform.position - oldPos).x;
        oldPos = transform.position;
        if (diff < 0)
        {
            animator.SetFloat("Horizontal", -1);
        } else if (diff > 0)
        {
            animator.SetFloat("Horizontal", 1);
        } else
        {
            animator.SetFloat("Horizontal", 0);
        }
        
    }

    private void EnemyMovement()
    {
        if (waypointIndex < waypoints.Count)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementFPSInd = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementFPSInd);
            if (transform.position == targetPosition)
            {
                if (!enemyPause)
                {
                    if (!start)
                    {
                        StartCoroutine(PauseEnemyMovement());
                    } else
                    {
                        animator.SetFloat("Idle", 1);
                    }
                } 
            } else
            {
                if (targetPosition.x - transform.position.x < 0)
                {
                    animator.SetFloat("Idle", 1);
                }
                else if (targetPosition.x - transform.position.x > 0)
                {
                    animator.SetFloat("Idle", -1);
                }
            }
        } else
        {
            waypointIndex = 0;
            Shuffle(waypoints);
        }
        start = false;
    }

    public void SetWaypoints(List<Transform> path)
    {
        waypoints = path;
    }

    public static List<T> Shuffle<T>(List<T> list)
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < list.Count; i++)
        {
            int k = rnd.Next(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }

    IEnumerator PauseEnemyMovement()
    {
        enemyPause = true;
        yield return new WaitForSeconds(enemyPauseTime);
        waypointIndex++;
        enemyPause = false;
    }
}
