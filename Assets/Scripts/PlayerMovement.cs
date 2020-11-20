﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    ////////////__Private__\\\\\\\\\\\\
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRender;

    ////////////__Public__\\\\\\\\\\\\
    public float walkSpeed;
    //__Player Information\\\\\\\\\\\\
    public int lifePoints;
    public int level;
    public int exp;

    //Basic Attack Information\\
    public int basicAttackDMG;
    public float basicAttackCD;
    public float basicAttackTimer = 0f;
    public GameObject BasicAttackPrefab;

    //Strong Attack Information\\
    public int strongAttackDMG;
    public float strongAttackCD;
    public float strongAttackTimer = 0f;
    public GameObject StrongAttackPrefab;

    //Shield Defense Information\\
    public int percentageDefense;
    public float shieldCD;
    public float shieldDefenseTimer = 0f;
    public GameObject ShieldPrefab;


    //Awake is called before the Start
    void Awake()
    {
        ////////////__Calling stuff__\\\\\\\\\\\\
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

        if (basicAttackTimer >= 0) 
        {
            basicAttackTimer -= Time.deltaTime;
        }

        if (strongAttackTimer >= 0)
        {
            strongAttackTimer -= Time.deltaTime;
        }

        if (shieldDefenseTimer >= 0)
        {
            shieldDefenseTimer -= Time.deltaTime;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ////////////__movimento X & Y__\\\\\\\\\\\\
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        ////////////__Attacks && Defenses__\\\\\\\\\\\\
        ///Melhorar codigo, tirar GetAxis
        float basicAttack = Input.GetAxis("Fire1");
        float strongAttack = Input.GetAxis("Fire2");
        float block = Input.GetAxis("Fire3");

        rb.velocity = new Vector2(horizontal * walkSpeed, vertical * walkSpeed);

        ////////////__FlipX__\\\\\\\\\\\\
        if ((horizontal > 0) && (spriteRender.flipX))
        {
            spriteRender.flipX = false;
        }
        else if ((horizontal < 0) && (!spriteRender.flipX))
        {
            spriteRender.flipX = true;
        }

        ////////////__ANIMATIONS__\\\\\\\\\\\\
        //__MOVEMENT X & Y__\\
        if (horizontal != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
        else if (vertical != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.y));
        }
        else
        {
            animator.SetFloat("Speed", -1);
        }

        //__BASIC ATTACK__\\
        if ((basicAttack != 0) && (basicAttackTimer <= 0))
        {
            //Animação
            animator.SetBool("BasicAttack", true);
            //Velocidade = 0
            rb.velocity = new Vector2(0, 0);

            //Create Object
            GameObject BasicAttack = Instantiate(BasicAttackPrefab, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity);
            basicAttackTimer = basicAttackCD;
        }
        else
        {
            animator.SetBool("BasicAttack", false);
        }

        //__STRONG ATTACK__\\
        if ((strongAttack != 0) && (strongAttackTimer <= 0))
        {
            //Animação
            animator.SetBool("StrongAttack", true);
            //Velocidade = 0
            rb.velocity = new Vector2(0,0);

            //Create Object
            GameObject StrongAttack = Instantiate(StrongAttackPrefab, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity);
            strongAttackTimer = strongAttackCD;

        }
        else
        {
            animator.SetBool("StrongAttack", false);
        }

        //__BLOCK && SPEED 0,0__\\
        if ((block != 0) && (shieldDefenseTimer <= 0))
        {
            //Animação
            animator.SetBool("Block", true);
            //Velocidade = 0
            rb.velocity = new Vector2(0,0);

            //Create Object
            GameObject SheildDefense = Instantiate(ShieldPrefab, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity);
            shieldDefenseTimer = shieldCD;
        }
        else 
        {
            animator.SetBool("Block", false);
        }


        //__DEATH__\\
        animator.SetInteger("Health", lifePoints);
    
    }
}