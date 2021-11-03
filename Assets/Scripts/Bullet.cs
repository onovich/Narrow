﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    private void OnEnable()
    {
        sprite.enabled = true;
        trail.SetActive(false);
        ifRebound = false;
        trail.GetComponent<TrailRenderer>().Clear();
    }


    public void SetDir(int dir)
    {
        direction = dir;
    }

    //public ParticleSystem explode;

    [Range(-1,1)]
    public int direction = 1;

    public float speed = 5f;
    Rigidbody2D rigid;

    //private LayerMask player;
    //private LayerMask wall;
    private LayerMask harmful;
    //ContactFilter2D playerFilter;
    //ContactFilter2D wallFilter;
    ContactFilter2D filter;

    Transform trans;

    SpriteRenderer sprite;

    public GameObject trail;
    TrailRenderer trailRenderer;
    RaycastCreater raycastCreater;
     void Awake()
    {
        trans = transform;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trailRenderer = trail.GetComponent<TrailRenderer>();
        SetDir(direction);
          
        raycastCreater = new RaycastCreater(trans);

    }

    [HideInInspector]
    public bool ifRebound = false;


    private void FixedUpdate()
    {
         Vector3 normalSpeed = (new Vector3(direction, 0, 0)).normalized * speed;
        if (normalSpeed != Vector3.zero)
        {
             rigid.velocity += (Vector2)normalSpeed * speed * Time.deltaTime;
            
        }
    }


    Vector3 dustbinPlace = new Vector3(20,20,0);


    void Hit(GameObject hit)
    {
        Debug.Log("击中" + hit.name);
        hit.GetComponent<Destructible>().GetHurt(GetComponent<Attack>().attackValue);
        CameraShake.instance.Shake();
        sprite.enabled = false;
        trans.position = dustbinPlace;
        trailRenderer.Clear();
     }


    // Update is called once per frame
    void Update()
    {

         float dis = .12f;
 

        GameObject hit = raycastCreater.Raycast("harmful",5, direction,dis,false);//ignoreSelf选false是因为考虑到会被发射出的子弹再反弹后伤害
 
        if (hit!=null)
        {
            Destructible destructible = hit.GetComponent<Destructible>();
            Fort fort = hit.GetComponent<Fort>();
            if (fort != null)
            {
                if (ifRebound)
                {
                    Hit(hit);
                   
                }
            }
            else if (destructible.harmful)
            {
                Hit(hit);
                
            }
            
           
        }
        else
        {
            //Debug.Log("未击中");
        }

        

    }
}
