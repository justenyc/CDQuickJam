﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rookmovement : MonoBehaviour
{

    public float speed = 3f;

    public float attackCoolDown = 1.5f;
    public float attackCoolDownTimer = 0f;
    public int damageDealt = 20;

    public float knockBackStrength = 2f;
    public float knockBackTime = 0.1f;

    public GameObject deathparticle;
    private Vector3 playerRelativePosition;
    private Vector2 moveDir;
    public Transform player;

    private Rigidbody2D rb;

    private Animator rookAnimator;

    public bool moving;

    private bool spawning = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rookAnimator = GetComponent<Animator>();
        attackCoolDownTimer = attackCoolDown;
        moving = false;
        player = FindObjectOfType<Player>().transform;
    }


    private void FixedUpdate()
    {
        try
        {
            playerRelativePosition = transform.InverseTransformPoint(player.position);

            if (attackCoolDownTimer < 0)
            {
                attackCoolDownTimer = 0;
            }

            if (attackCoolDownTimer > 0 && moving == false)
                attackCoolDownTimer -= Time.deltaTime;

            if (attackCoolDownTimer == 0 && moving == false && spawning == false)
            {
                if (Mathf.Abs(playerRelativePosition.x) < (Mathf.Abs(playerRelativePosition.y)) && moving == false)
                {
                    if (playerRelativePosition.y >= 0)
                    {
                        MoveUp();
                    }
                    else
                    {
                        MoveDown();
                    }
                }
                else if (Mathf.Abs(playerRelativePosition.x) > (Mathf.Abs(playerRelativePosition.y)) && moving == false)
                {
                    if (playerRelativePosition.x >= 0)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
            }
        }
        catch
        {
            Debug.Log("Player Not Found");
        }
    }

    private void MoveUp()
    {
        rookAnimator.SetTrigger("MoveUp");
        moveDir = new Vector2(0, 1 * speed);
        moving = true;
        rb.velocity = moveDir;
    }

    private void MoveDown()
    {
        rookAnimator.SetTrigger("MoveDown");
        moveDir = new Vector2(0, -1 * speed);
        moving = true;
        rb.velocity = moveDir;
    }

    private void MoveRight()
    {
        moveDir = new Vector2(1 * speed, 0);
        moving = true;
        rb.velocity = moveDir;
    }

    private void MoveLeft()
    {
        moveDir = new Vector2(-1 * speed, 0);
        moving = true;
        rb.velocity = moveDir;
    }

    IEnumerator Stop()
    {
        rookAnimator.SetTrigger("Idle");
        moving = false;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(moveDir * -1 * knockBackStrength, ForceMode2D.Impulse);
        attackCoolDownTimer = attackCoolDown;

        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = new Vector2(0, 0);
    }

    public void SetSpawnFalse()
    {
        spawning = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Walls")) || (collision.gameObject.CompareTag("Obstacles")))
        {
            try
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageDealt);
                StartCoroutine("Stop");
            }
            catch
            {
                StartCoroutine("Stop");
            }
        }
    }

    private void OnDestroy()
    {
        //Instantiate(deathparticle, transform.position, Quaternion.identity);
    }
}
