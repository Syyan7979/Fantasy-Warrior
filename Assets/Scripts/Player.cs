using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Config Params
    [Range(1, 10)][SerializeField] float movementSpeed = 6.5f;
    [SerializeField] float animationSpeed = 0.35f;
    [SerializeField] float hitAnimationTime = 0.188f;
    [SerializeField] float shieldRegenTime = 3.5f;
    [SerializeField] float playerHealth = 100;
    [SerializeField] float playerShield = 80;
    [SerializeField] float deathAnimationTime = 1f;
    [SerializeField] GameObject attackRight, attackLeft, healthBar, shieldBar;

    // Initial Config
    Vector2 movement;
    Rigidbody2D rb;
    Animator animator;
    Slider sliderHealth, sliderShield;
    bool shieldRegen = true;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sliderHealth = healthBar.GetComponent<Slider>();
        sliderShield = shieldBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerAttack();
        Idle();
        SetSliderValues();
        AutoShieldRegeneration();
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        if (playerHealth == 0 & !dead)
        {
            dead = true;
            StartCoroutine(DeathAnimation());
        }
    }

    private void AutoShieldRegeneration()
    {
        if (shieldRegen)
        {
            StartCoroutine(ShieldRegen());
            playerShield = Mathf.Clamp(playerShield + 10, 0, 100);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
        
    private void PlayerAttack()
    {
        if (Input.GetButtonDown("Attack1") && !animator.GetBool("Attack"))
        {
            StartCoroutine(AttackCoroutine());
            if (movement.x < 0 || animator.GetFloat("Idle") == -1)
            {
                GameObject attack = Instantiate(attackLeft, transform.position, Quaternion.identity);
                Destroy(attack, animationSpeed);
            }
            else
            {
                GameObject attack = Instantiate(attackRight, transform.position, Quaternion.identity);
                Destroy(attack, animationSpeed);
            }
        } 
    }

    IEnumerator AttackCoroutine()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(animationSpeed);
        animator.SetBool("Attack", false);
    }

    private void PlayerMove()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void Idle()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetFloat("Idle", -1);
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            animator.SetFloat("Idle", 1);
        }
    }

    public void OnTriggerEnter2D(Collider2D attack)
    {
        if (attack.tag == "Enemy_Attack")
        {
            WhenAttacking whenAttacking = attack.GetComponent<WhenAttacking>();
            if (playerShield == 0)
            {
                playerHealth = Mathf.Clamp(playerHealth - whenAttacking.GetDamage(), 0, 100); ;
            } else
            {
                playerShield = Mathf.Clamp(playerShield - whenAttacking.GetDamage(),0, 100);
            }
            
            if (!animator.GetBool("Hit"))
            {
                StartCoroutine(HitAnimation());
            }
        }
    }

    IEnumerator HitAnimation()
    {
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(hitAnimationTime);
        animator.SetBool("Hit", false);
    }

    void SetSliderValues()
    {
        sliderHealth.value = playerHealth / 100;
        sliderShield.value = playerShield / 100;
    }

    IEnumerator ShieldRegen()
    {
        shieldRegen = false;
        yield return new WaitForSeconds(shieldRegenTime);
        shieldRegen = true;
        
    }

    IEnumerator DeathAnimation()
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(deathAnimationTime);
        animator.SetBool("Dead", false);
    }
}
