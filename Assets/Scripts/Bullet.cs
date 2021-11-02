using System.Collections;
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

    RaycastCreater raycastCreater;

    void Awake()
    {
        trans = transform;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
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


    // Update is called once per frame
    void Update()
    {

         float dis = .12f;
 

        GameObject hit = raycastCreater.Raycast("harmful",5, direction,dis);
 
        if (hit!=null)
        {
            Destructible destructible = hit.GetComponent<Destructible>();
            Fort fort = hit.GetComponent<Fort>();
            if (fort != null)
            {
                if (ifRebound)
                {
                    Debug.Log("击中" + hit.name);
                    hit.GetComponent<Destructible>().GetHurt(GetComponent<Attack>().attackValue);
                    CameraShake.instance.Shake();
                    sprite.enabled = false;
                }
            }
            else if (destructible.harmful)
            {
                Debug.Log("击中" + hit.name);
                hit.GetComponent<Destructible>().GetHurt(GetComponent<Attack>().attackValue);
                CameraShake.instance.Shake();
                sprite.enabled = false;
            }
            
           
        }
        else
        {
            //Debug.Log("未击中");
        }

        

    }
}
