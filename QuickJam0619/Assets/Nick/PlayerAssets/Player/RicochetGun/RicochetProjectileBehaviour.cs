﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetProjectileBehaviour : MonoBehaviour
{

    public AudioClip clip;
    public AudioSource audSource;

    private Vector2 target;
    private Vector2 playerPos;

    private Vector2 projectilePos;

    private Vector2 projectileVelocity;
    private Vector2 projectileDirection;

    public float speed;

    public float projectileLifeTime;

    public int DamageDealt;

    // Start is called before the first frame update
    void Start()
    {
        audSource = this.GetComponent<AudioSource>();

        //This part took me forever to understand, if you don't include a z value here it will always return the 
        //position of the camera
        target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;

        projectileVelocity = target - playerPos;
        projectileDirection = projectileVelocity.normalized;

        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Despawn the projectile after a little while
        Destroy(gameObject, projectileLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        projectilePos = transform.position;
        transform.position = projectilePos + (projectileDirection * speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlaySound();

        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(DamageDealt);
        }

        projectileDirection = Vector2.Reflect(projectileDirection, collision.contacts[0].normal);
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void PlaySound()
    {
        audSource.clip = clip;
        audSource.Stop();
        audSource.Play();
    }
}
