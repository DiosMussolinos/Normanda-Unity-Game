﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShots : MonoBehaviour
{
    //Set speed
    public float speed;

    //Math teacher asked about it
    public float randomX;
    public float randomY;

    private float YOffSet = 0.5f;
    private Transform player;
    private Vector2 target;

    //private Animator animator;

    // Awake is called before Start
    void Awake() {

        randomX = Random.Range(-1.1f, 1.1f);
        randomY = Random.Range(-1.1f, 1.1f);

        //Find the player
        player = GameObject.FindWithTag("Player").transform;
        //Initial position of the player 
        target = new Vector2(player.position.x + (randomX), player.position.y + YOffSet + (randomY));
    }

    //Update is called once per frame
    void Update()
    {
        //movement of the Shot
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        //Hits the initial position of the player, BOOM, GET REK
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collide with the player, BOOM, GET FUCKED
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destruir
            Destroy(gameObject);
        }

        //Collide with the player, BOOM, GET UWU
        if (collision.gameObject.CompareTag("Shield"))
        {
            //Destruir
            Destroy(gameObject);
        }

        //Collide with the player, BOOM, GET DELETED
        if (collision.gameObject.CompareTag("TileCollision"))
        {
            //Destruir
            Destroy(gameObject);
        }

        //Collide with Final Boss
        if (collision.gameObject.CompareTag("FinalBoss"))
        {
            //Destruir
            Destroy(gameObject);
        }
    }

}
