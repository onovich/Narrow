﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private Global global;
    private GameObject player;
    public Rigidbody2D rigid;
    public ParticleSystem explode;
    public TrailRenderer trail;

    private void Start()
    {
        init();

        
    }

    private void init()
    {
        global = Global.instance;
        player = global.player;
        //sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        trail.Clear();
        Move();
    }

    public void Move()
    {
        init();
        //sprite.enabled = true;

        Vector3 dir = (transform.position - player.transform.position).normalized;
        dir.z = 0;

        StartCoroutine(Moving(dir));
        //rigid.velocity += (Vector2)dir * speed;

        

    }

    IEnumerator Moving(Vector3 dir)
    {
        while (true)
        {
            //transform.position += dir * speed;
            //rigid.velocity += (Vector2)dir * speed * Time.smoothDeltaTime;
            //rigid.velocity = (Vector2)dir * speed;
            rigid.position+= (Vector2)dir * speed * Time.smoothDeltaTime;
            yield return null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16 )
        {
             //Debug.Log("碰撞");
            //explode.Play();
            Destroy();
        }
    }
    private void Destroy()
    {
        //sprite.enabled = false;
        gameObject.SetActive(false);
    }

}
