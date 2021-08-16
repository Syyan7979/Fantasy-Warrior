using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPathing : MonoBehaviour
{
    // Config Params
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float movementSpeed = 4.5f;
    //[SerializeField] float enemyPauseTime = 1.2f;

    // Dynamic Variables
    int waypointIndex = 0;
    Animator animator;
    Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        Shuffle(waypoints);
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
                waypointIndex ++;
            }
        } else
        {
            waypointIndex = 0;
            Shuffle(waypoints);
        }
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
}
