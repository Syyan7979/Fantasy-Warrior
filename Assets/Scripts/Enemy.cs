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

    // Dynamic Variables
    Animator animator;
    EnemySpawner enemySpawner;
    EnemyPathing enemyPathing;
    Player player;
    Vector3 oldPos;
    bool dead = false;
    float diff;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
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
        if (distanceBetween < 2)
        {
            if (diff < 0)
            {
                GameObject attack = Instantiate(enemyAttackLeft, transform.position, Quaternion.identity);
                Destroy(attack, animationAttackTime);
            }
            else if (diff > 0)
            {
                GameObject attack = Instantiate(enemyAttackRight, transform.position, Quaternion.identity);
                Destroy(attack, animationAttackTime);
            }
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
        float distanceBetween = Vector2.Distance(player.transform.position, transform.position);
        if (distanceBetween < 7)
        {
            enemyPathing.enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
            
        } else
        {
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
}
