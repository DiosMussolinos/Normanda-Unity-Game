﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    //Behavior
    public float speed;
    public float stoppingDistance;
    public float retreaDistance;
    public float visionDistance;
   
    //Control of shots
    public float timeBtwShots;
    public float startTimeBtwShots;

    //Details of Enemy
    public int archerLife;
    public int projectileDamage;
    public int gold;
    public int experience;
    public int level;
    
    //Calling stuff
    public GameObject projectile;
    public Transform player;

    // Awake is called before Start
    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > stoppingDistance)
        {
            transform.position = this.transform.position;
        }
        else if (distance < retreaDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }


        if ((timeBtwShots <= 0) && (distance < visionDistance))
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        if (archerLife <= 0) 
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BasicAttack"))
        {
            archerLife = archerLife - 10;
        }

        if (collision.gameObject.CompareTag("StrongAttack"))
        {
            archerLife = archerLife - 20;
        }

    }

}