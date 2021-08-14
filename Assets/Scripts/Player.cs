using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config Params
    [Range(1, 10)][SerializeField] float movementSpeed;

    // Initial Config
    Vector2 movement;
    Rigidbody2D rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        PlayerMove();
        PlayerAttack();
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    IEnumerator AttackCoroutine()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.467f);
        animator.SetBool("Attack", false);
    }

    private void PlayerAttack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(AttackCoroutine());
        } 
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
}
