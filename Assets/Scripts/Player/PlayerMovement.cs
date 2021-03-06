﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    ////////////__Private__\\\\\\\\\\\\
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRender;
    
    ////////////__Public__\\\\\\\\\\\\
    public GameObject BasicAttackPrefab;
    public GameObject StrongAttackPrefab;
    public GameObject ShieldPrefab;

    //Backend stuff
    public string BaseAPI = "http://localhost:3909";

    //Awake is called before the Start
    void Awake()
    {
        ////////////__Calling stuff__\\\\\\\\\\\\
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        DontDestroyOnLoad(this.gameObject);

        Application.targetFrameRate = 144;

    }

    // Update is called once per frame
    void Update()
    {

        ////////////__movimento X & Y__\\\\\\\\\\\\
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        ////////////__Attacks && Defenses__\\\\\\\\\\\\
        
        float basicAttack = Input.GetAxis("Fire1");
        float strongAttack = Input.GetAxis("Fire2");
        float block = Input.GetAxis("Fire3");


        ////////////////////////__CHEAT CODES\\\\\\\\\\\\\\\\\\\\\\\\

        if (Input.GetKeyDown(KeyCode.L))
        {
            SourceCode.lifePoints = SourceCode.maxLifePoints;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SourceCode.playerGold += 100;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SourceCode.playerExp += 60;
        }
        ////////////////////////__CHEAT CODES\\\\\\\\\\\\\\\\\\\\\\\\


        // Life > 0 then MOVE, YOU ARE ON MY WAY
        if (SourceCode.lifePoints > 0)
        {
            rb.velocity = new Vector2(horizontal * SourceCode.walkSpeed, vertical * SourceCode.walkSpeed);

            //Debug.Log(rb.velocity);
            ////////////__FlipX__\\\\\\\\\\\\
            if ((horizontal > 0) && (spriteRender.flipX))
            {
                spriteRender.flipX = false;
            }
            else if ((horizontal < 0) && (!spriteRender.flipX))
            {
                spriteRender.flipX = true;
            }
        } 
        else 
        {
            rb.velocity = new Vector2(0, 0);
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

        //__MOVEMENT X & Y__\\
        //__BASIC ATTACK__\\
        if ((basicAttack != 0) && (SourceCode.basicAttackTimer <= 0))
        {
            //Animação
            animator.SetBool("BasicAttack", true);
            

            ////Create Object & Timer To create\\\\
                if (spriteRender.flipX) {
                    Invoke("BasicAttackRight", 0.15f);
                }
                if (!spriteRender.flipX)
                {
                    Invoke("BasicAttackLeft", 0.15f);
                }
            

            //Reset Timer
            SourceCode.basicAttackTimer = SourceCode.basicAttackCD;

        } else if(SourceCode.basicAttackTimer > 0)
        {
            //Return To Idle or others
            animator.SetBool("BasicAttack", false);
    
        }


        //__STRONG ATTACK__\\
        if ((strongAttack != 0) && (SourceCode.strongAttackTimer <= 0))
        {
            //Animação
            animator.SetBool("StrongAttack", true);
            
            ////Create Object & Timer To create\\\\
            if (spriteRender.flipX)
            {
                Invoke("StrongAttackRight", 0.2f);
            }
            if (!spriteRender.flipX)
            {
                Invoke("StrongAttackLeft", 0.2f);
            }
            
            //Reset Timer
            SourceCode.strongAttackTimer = SourceCode.strongAttackCD;

        } else if (SourceCode.strongAttackTimer > 0)
        {
            //Return To Idle or others
            animator.SetBool("StrongAttack", false);
        }

        //__BLOCK__\\
        if ((block != 0) && (SourceCode.blockInstantiate != true))
        {
            //Animação
            animator.SetBool("Shield", true);

            ////Create Object & Timer To create\\\\
            if (spriteRender.flipX)
            {
                GameObject SheildDefenseRight = Instantiate(ShieldPrefab, new Vector3(transform.position.x - 0.83f, transform.position.y + 1), Quaternion.identity);
            }
            if (!spriteRender.flipX)
            {
                GameObject SheildDefenseLeft = Instantiate(ShieldPrefab, new Vector3(transform.position.x + 0.75f, transform.position.y + 1), Quaternion.identity);
            }
            //if(blockInstantiate = true) { DONT INTANTIATE AGAIN }
            SourceCode.blockInstantiate = true;
        }

        if  ((block == 0) && (SourceCode.blockInstantiate = true))
        {
            //Destroy shield prefab instanciate
            animator.SetBool("Shield", false);
            SourceCode.blockInstantiate = false;
        }


        //__DEATH__\\
        if (SourceCode.lifePoints <= 0) 
        {
            //Ridigbody OFF
            rb.bodyType = RigidbodyType2D.Kinematic;
            //ANIMAÇÃO
            animator.SetBool("Death", true);
        } else 
        {
            //Rididbody ON
            rb.bodyType = RigidbodyType2D.Dynamic;
            //ANIMAÇÃO
            animator.SetBool("Death", false);
        }

        if(SourceCode.lifePoints > SourceCode.maxLifePoints) 
        {
            SourceCode.lifePoints = SourceCode.maxLifePoints;
        }

        if (SourceCode.lifePoints < 0)
        {
            SourceCode.lifePoints = 0;
        }
            Timer();
    }
    //__END UPDATE__\\
    ////////////////////__INVOKE RELATED__\\\\\\\\\\\\\\\\\\\\
    void BasicAttackRight()
    {
        GameObject BasicAttackRight = Instantiate(BasicAttackPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y + 1), Quaternion.identity);
    }

    void BasicAttackLeft()
    {
        GameObject BasicAttackLeft = Instantiate(BasicAttackPrefab, new Vector3(transform.position.x + 1.3f, transform.position.y + 1), Quaternion.identity);
    }

    void StrongAttackRight()
    {
        GameObject StrongAttackRight = Instantiate(StrongAttackPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y + 1), Quaternion.identity);
    }

    void StrongAttackLeft()
    {
        GameObject StrongAttackLeft = Instantiate(StrongAttackPrefab, new Vector3(transform.position.x + 1.3f, transform.position.y + 1), Quaternion.identity);
    }
    ////////////////////__INVOKE RELATED__\\\\\\\\\\\\\\\\\\\\
    
    private void Timer() 
    {
        //__ANIMATION TIMER__\\
        //Basic Attack
        if (SourceCode.basicAttackTimer >= 0)
        {
            SourceCode.basicAttackTimer -= Time.deltaTime;
        }

        //Strong Attack
        if (SourceCode.strongAttackTimer >= 0)
        {
            SourceCode.strongAttackTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //__Collision With Archer Shot__\\
        if (collision.gameObject.CompareTag("Shot"))
        {
            SourceCode.lifePoints -= SourceCode.projectileDamage;
            //UPDATE BACKEND HERE
            string jsonstring = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints, SourceCode.userID));
            StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstring));
        }

        //__Collision With Soldier__\\
        if (collision.gameObject.CompareTag("Soldier"))
        {
            SourceCode.lifePoints -= SourceCode.soldierCollisionDamage;
            //UPDATE BACKEND HERE
            string jsonstring = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints, SourceCode.userID));
            StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstring));
        }

        //__Collision With Soldier Attack__\\
        if (collision.gameObject.CompareTag("SoldierAttack"))
        {
            if (SourceCode.blockInstantiate == true)
            {
                SourceCode.lifePoints = (int)(SourceCode.lifePoints - (SourceCode.soldierDamage - SourceCode.percentageDefense/10));
                //UPDATE BACKEND HERE
                string jsonstring = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints, SourceCode.userID));
                StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstring));
            }
            else
            {
                SourceCode.lifePoints -= SourceCode.soldierDamage;
                //UPDATE BACKEND HERE
                string jsonstringexp = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints, SourceCode.userID));
                StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstringexp));
            }
        }

        //__Collision With Final Boss__\\
        if (collision.gameObject.CompareTag("FinalBoss"))
        {
            SourceCode.lifePoints -= SourceCode.finalBossTouchDamage;
            //UPDATE BACKEND HERE
            string jsonstring = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints, SourceCode.userID));
            StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstring));
        }

        //__Collision With Final Boss Attack__\\
        if (collision.gameObject.CompareTag("FinalBossAttack"))
        {
            if (SourceCode.blockInstantiate == true)
            {
                SourceCode.lifePoints = (int)(SourceCode.lifePoints - (SourceCode.finalBossAttackDamage - SourceCode.percentageDefense / 10));
                //UPDATE BACKEND HERE
                string jsonstring = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints, SourceCode.userID));
                StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstring));
            }
            else
            {
                SourceCode.lifePoints -= SourceCode.finalBossAttackDamage;
                //UPDATE BACKEND HERE
                string jsonstring = JsonUtility.ToJson(new PlayerNewHp(SourceCode.lifePoints ,SourceCode.userID));
                StartCoroutine(UpdateLife(BaseAPI + "/updateLife", jsonstring));
            }
        }

        //ONLY one IEnumerator to post the new life points
        IEnumerator UpdateLife(string url, string json)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
            byte[] jsonbyte = new System.Text.UTF8Encoding().GetBytes(json);

            webRequest.uploadHandler = new UploadHandlerRaw(jsonbyte);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                SourceCode.lifePoints = int.Parse(webRequest.downloadHandler.text);
            }
        }
        
    }
    
    /*
    ⢀⣠⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⣠⣤⣶⣶
    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⢰⣿⣿⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣀⣀⣾⣿⣿⣿⣿
    ⣿⣿⣿⣿⣿⡏⠉⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿
    ⣿⣿⣿⣿⣿⣿⠀⠀⠀⠈⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⠉⠁⠀⣿
    ⣿⣿⣿⣿⣿⣿⣧⡀⠀⠀⠀⠀⠙⠿⠿⠿⠻⠿⠿⠟⠿⠛⠉⠀⠀⠀⠀⠀⣸⣿
    ⣿⣿⣿⣿⣿⣿⣿⣷⣄⠀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⣴⣿⣿⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⢰⣹⡆⠀⠀⠀⠀⠀⠀⣭⣷⠀⠀⠀⠸⣿⣿⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠈⠉⠀⠀⠤⠄⠀⠀⠀⠉⠁⠀⠀⠀⠀⢿⣿⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⢾⣿⣷⠀⠀⠀⠀⡠⠤⢄⠀⠀⠀⠠⣿⣿⣷⠀⢸⣿⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⡀⠉⠀⠀⠀⠀⠀⢄⠀⢀⠀⠀⠀⠀⠉⠉⠁⠀⠀⣿⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⣿⣿
    ⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿
    When your spaghetthi code works
        & The professor likes
    */

}
