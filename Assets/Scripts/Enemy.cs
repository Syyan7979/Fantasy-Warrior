using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Config Params
    [SerializeField] int enemyHealth = 15;
    [SerializeField] float hitAnimationTime = 0.133f;
    [SerializeField] float deathAnimationTime = 0.4f;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] GameObject enemyAttackRight, enemyAttackLeft;
    [SerializeField] float animationAttackTime = 0.35f;
    [SerializeField] float attackInterval = 5f;

    // Dynamic Variables
    Animator animator;
    EnemySpawner enemySpawner;
    EnemyPathing enemyPathing;
    Player player;
    RoomLock roomLock;
    Vector3 oldPos;
    bool dead = false;
    float diff;
    bool attackStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyPathing = GetComponent<EnemyPathing>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        DeathChecker();
        FollowPlayer();
        Increment();
        EnemyAttacksConfig();
    }

    private void EnemyAttacksConfig()
    {
        float distanceBetween = Vector2.Distance(player.transform.position, transform.position);
        if (distanceBetween < 2 && !attackStopped)
        {
            StartCoroutine(AnimateAttack());
            StartCoroutine(EnemyAttackHandler());
        }
        
    }

    public void OnTriggerEnter2D(Collider2D attack)
    {
        if(attack.tag == "Attack")
        {
            WhenAttacking whenAttacking = attack.GetComponent<WhenAttacking>();
            enemyHealth -= whenAttacking.GetDamage();
            if (!animator.GetBool("Hit"))
            {
                StartCoroutine(HitAnimation());
            }
        }
    }

    void DeathChecker()
    {
        if (enemyHealth <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(DeathActionHandler());
        }
    }

    void FollowPlayer()
    {
        System.Random rnd = new System.Random();
        float distanceBetween = Vector2.Distance(player.transform.position, transform.position);
        Vector3 offset = new Vector2();
        bool forIdle = false;
        float distanceBetweenX;
        if (distanceBetween < 7 && roomLock.ReturnLockedState())
        {
            if (!forIdle)
            {
                distanceBetweenX = transform.position.x - player.transform.position.x;
                if (distanceBetweenX < 0)
                {
                    animator.SetFloat("Idle", -1);
                    offset = new Vector2(-1, 0);
                } else if (distanceBetweenX > 0)
                {
                    animator.SetFloat("Idle", 1);
                    offset = new Vector2(1, 0);
                }
                forIdle = true;
            }
            enemyPathing.enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position + offset, movementSpeed * Time.deltaTime);
        } else
        {
            forIdle = false;
            enemyPathing.enabled = true;
        }
    }

    private void Increment()
    {
        diff = (transform.position - oldPos).x;
        oldPos = transform.position;
        if (diff < 0)
        {
            animator.SetFloat("Horizontal", -1);
        }
        else if (diff > 0)
        {
            animator.SetFloat("Horizontal", 1);
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
        }

    }

    IEnumerator HitAnimation()
    {
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(hitAnimationTime);
        animator.SetBool("Hit", false);
    }

    IEnumerator DeathActionHandler()
    {
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(deathAnimationTime);
        enemySpawner.SpawnKilled();
        Destroy(gameObject);
    }

    IEnumerator EnemyAttackHandler()
    {
        attackStopped = true;
        if (diff < 0 || (animator.GetFloat("Horizontal") == 0 && animator.GetFloat("Idle") == 1))
        {
            GameObject attack = Instantiate(enemyAttackLeft, transform.position, Quaternion.identity);
            Destroy(attack, animationAttackTime);
        }
        else if (diff > 0 || (animator.GetFloat("Horizontal") == 0 && animator.GetFloat("Idle") == -1))
        {
            GameObject attack = Instantiate(enemyAttackRight, transform.position, Quaternion.identity);
            Destroy(attack, animationAttackTime);
        }
        yield return new WaitForSeconds(attackInterval);
        attackStopped = false;
    }

    IEnumerator AnimateAttack()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(animationAttackTime);
        animator.SetBool("Attack", false);
    }

    public void SetRoomLock(RoomLock setRoomLock)
    {
        roomLock = setRoomLock;
    }

    public void SetEnemySpawnerHandler(EnemySpawner getEnemySpawner)
    {
        enemySpawner = getEnemySpawner;
    }
}
