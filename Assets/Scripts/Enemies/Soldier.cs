﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public int soldierLife;

    //Calling stuff
    private Rigidbody2D rb;
    public Transform player;
    private float soldierScale;
    private SpriteRenderer spriteRender;
    private Animator animator;

    public GameObject SoldierAttack;

    public float timeBtwAttacks = 2f;
    public float startTimeBtwAttacks = 5f;

    // Awake is called before Start
    void Awake()
    {
        //RigidBody
        rb = GetComponent<Rigidbody2D>();

        //Find Player
        player = GameObject.FindWithTag("Player").transform;

        //Algum dia eu lembro o que é isso - Gabriel - 13/12/2020
        soldierScale = (transform.localScale.x / 2) + 1f;
        
        //Fazer o flip
        spriteRender = GetComponent<SpriteRenderer>();
        
        //Controle de animação
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Distance from Enemy and Player
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        //Position of The Attacks
        Vector3 negativeSpawAttack = new Vector3(transform.position.x - 0.85f, transform.position.y, transform.position.z);
        Vector3 positiveSpawAttack = new Vector3(transform.position.x + 0.85f, transform.position.y, transform.position.z);
        
        //Distance From The player in the X to Flip    
        float distanceXFromPlayer = transform.position.x - player.transform.position.x;

   
        //__BEHAVIOR__\\
        if (distance < SourceCode.soldierVisionDistance && distance > 2)
        {
            //Animation
            animator.SetBool("Walk", true);

            //Go to the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, SourceCode.soldierSpeed * Time.deltaTime);
        } 
        else
        {
            //Animation
            animator.SetBool("Walk", false);
        }
        //__BEHAVIOR__\\


        //Timer && distance = Kill that mf
        if ((timeBtwAttacks <= 0) && (distance < 2.5f))
        {
            //Animation
            animator.SetBool("Attack", true);

            //Fazer ser child
            Instantiate(SoldierAttack, negativeSpawAttack, Quaternion.identity);
            Instantiate(SoldierAttack, positiveSpawAttack, Quaternion.identity);
            

            timeBtwAttacks = startTimeBtwAttacks;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
            //Animation
            animator.SetBool("Attack", false);
        }

        //Turn Around That ass
        if (distanceXFromPlayer > 0)
        {
            //Turn around
            spriteRender.flipX = true;
        }
        else 
        {
            //Turn around
            spriteRender.flipX = false;
        }

        
        //Death
        if (soldierLife <= 0)
        {
            SourceCode.playerGold = SourceCode.playerGold + SourceCode.soldierGold;
            SourceCode.playerExp = SourceCode.playerExp + SourceCode.soldierExp;
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Colision basic attack
        if (collision.gameObject.CompareTag("BasicAttack"))
        {
            //Recieve DMG BasicAttack
            soldierLife = soldierLife - SourceCode.basicAttackDMG;
        }

        //Colision Strong attack
        if (collision.gameObject.CompareTag("StrongAttack"))
        {
            //Recieve DMG strong
            soldierLife = soldierLife - SourceCode.strongAttackDMG;
        }
    }

    /*
     ⡆⣐⢕⢕⢕⢕⢕⢕⢕⢕⠅⢗⢕⢕⢕⢕⢕⢕⢕⠕⠕⢕⢕⢕⢕⢕⢕⢕⢕⢕
  ⢐⢕⢕⢕⢕⢕⣕⢕⢕⠕⠁⢕⢕⢕⢕⢕⢕⢕⢕⠅⡄⢕⢕⢕⢕⢕⢕⢕⢕⢕
  ⢕⢕⢕⢕⢕⠅⢗⢕⠕⣠⠄⣗⢕⢕⠕⢕⢕⢕⠕⢠⣿⠐⢕⢕⢕⠑⢕⢕⠵⢕
  ⢕⢕⢕⢕⠁⢜⠕⢁⣴⣿⡇⢓⢕⢵⢐⢕⢕⠕⢁⣾⢿⣧⠑⢕⢕⠄⢑⢕⠅⢕
  ⢕⢕⠵⢁⠔⢁⣤⣤⣶⣶⣶⡐⣕⢽⠐⢕⠕⣡⣾⣶⣶⣶⣤⡁⢓⢕⠄⢑⢅⢑
  ⠍⣧⠄⣶⣾⣿⣿⣿⣿⣿⣿⣷⣔⢕⢄⢡⣾⣿⣿⣿⣿⣿⣿⣿⣦⡑⢕⢤⠱⢐
  ⢠⢕⠅⣾⣿⠋⢿⣿⣿⣿⠉⣿⣿⣷⣦⣶⣽⣿⣿⠈⣿⣿⣿⣿⠏⢹⣷⣷⡅⢐
  ⣔⢕⢥⢻⣿⡀⠈⠛⠛⠁⢠⣿⣿⣿⣿⣿⣿⣿⣿⡀⠈⠛⠛⠁⠄⣼⣿⣿⡇⢔
  ⢕⢕⢽⢸⢟⢟⢖⢖⢤⣶⡟⢻⣿⡿⠻⣿⣿⡟⢀⣿⣦⢤⢤⢔⢞⢿⢿⣿⠁⢕
  ⢕⢕⠅⣐⢕⢕⢕⢕⢕⣿⣿⡄⠛⢀⣦⠈⠛⢁⣼⣿⢗⢕⢕⢕⢕⢕⢕⡏⣘⢕
  ⢕⢕⠅⢓⣕⣕⣕⣕⣵⣿⣿⣿⣾⣿⣿⣿⣿⣿⣿⣿⣷⣕⢕⢕⢕⢕⡵⢀⢕⢕
  ⢑⢕⠃⡈⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢃⢕⢕⢕
  ⣆⢕⠄⢱⣄⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⢁⢕⢕⠕⢁
  ⣿⣦⡀⣿⣿⣷⣶⣬⣍⣛⣛⣛⡛⠿⠿⠿⠛⠛⢛⣛⣉⣭⣤⣂⢜⠕⢑⣡⣴⣿
    CODE OPTIMIZATION DONE WITH PAIN
     */

}
