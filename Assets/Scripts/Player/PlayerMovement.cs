using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Transform trans;
    private Vector3 pos { get { return trans.position; } }

    [HideInInspector]
    public bool movable = true;
    public PlayerSetting setting;

    private float Horizontal;
    private Vector3 normalSpeed;
    private float speed{ get{return setting.moveSpeed; } }

    private Rigidbody2D rigid{get{ return GetComponent<Rigidbody2D>(); }}

    private Vector3 speedUpScale { get { return (Vector3)setting.speedUpScale + Vector3.forward; } }

    public TrailRenderer trail;

    private float trailWidth { get { return setting.trailWidth; } }
    private float trailWidthScale { get { return setting.trailWidthScale; } }

    private LayerMask obb;
    ContactFilter2D filter;
    public int hitNumber = 0;
    Vector3 originalCameraPos;
    private void Start()
    {
        trans = transform;
        obb = LayerMask.GetMask("OBB");
        filter = new ContactFilter2D
        {
            useLayerMask = true,
            //useTriggers = false,
            useTriggers = true,
            layerMask = obb,

        };

        collide = new UnityAction(onCollide);
        collideEvent.AddListener(collide);
        originalCameraPos = Camera.main.transform.position;

    }

    UnityAction collide;
    UnityEvent collideEvent = new UnityEvent();

    bool onMoving = false;
    bool onCollidering = false;


    public ParticleSystem particle;


    IEnumerator CameraShake()
    {
        Vector3 offset = new Vector3(0.02f, 0.02f,0f);
        Camera.main.transform.position = originalCameraPos+offset;
        yield return new WaitForSeconds(.1f);
        Camera.main.transform.position = originalCameraPos;
    }


    void onCollide()
    {
        Debug.Log("碰撞！");
        //rigid.velocity = new Vector3(0, 0, 0);

        //rigid.velocity = new Vector3(0, 0, 0);
        //Vector2 offset = Horizontal > 0 ? trans.right : -trans.right;
        //rigid.AddForce(-offset * 400);

        if (particle.isPlaying)
        {
            particle.Stop();
        }
        particle.Play();

        StopCoroutine(CameraShake());
        StartCoroutine(CameraShake());



        trail.Clear();

        if (onMoving)
        {
            stop = true;

            //除重
            if (trans.localScale != Vector3.one)
            {
                //trans.localScale = Vector3.Lerp(trans.localScale, speedUpScale, .1f);
                trans.DOScale(Vector3.one, .5f);
                trail.DOResize(trailWidth, .01f, .5f);

            }
        }

        
    }

    bool stop = true;

    void Update()
    {
        if (movable)
        {
            if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D)))
            {
                stop = false;
            }

            if (!stop)
            {
                //角色移动控制
                Horizontal = Input.GetAxis("Horizontal");
                normalSpeed = (new Vector3(Horizontal, 0, 0)).normalized;
                if (normalSpeed != Vector3.zero)
                {
                    rigid.velocity += (Vector2)normalSpeed * speed * Time.smoothDeltaTime;
                }
            }
            
            

            //角色形变控制
            //移动时:未因碰撞停止且未碰撞，且有按加速键
            if (((!stop) &&(!onCollidering))&&(Horizontal!=0))
            {
                //缩小拉长
                onMoving = true;
                //除重
                if(trans.localScale != speedUpScale)
                {
                    //trans.localScale = Vector3.Lerp(trans.localScale, speedUpScale, .1f);
                    trans.DOScale(speedUpScale,.5f);
                    trail.DOResize(trailWidthScale, .01f, .5f);
                }
            }
            //未移动时
            else
            {
                //变回原样
                onMoving = false;
                //除重
                if (trans.localScale != Vector3.one)
                {
                    //trans.localScale = Vector3.Lerp(trans.localScale, speedUpScale, .1f);
                    trans.DOScale(Vector3.one, .5f);
                    trail.DOResize(trailWidth, .01f, .5f);

                }

            }

            //角色碰撞检测
            RaycastHit2D[] hits = new RaycastHit2D[36];
            float dis = .1f;
            Vector2 offset = Horizontal > 0 ? trans.right : -trans.right;
            hitNumber = Physics2D.Raycast(trans.position, offset, filter, hits, dis);

            if (hitNumber > 0)
            {
                onCollidering = true;
                collideEvent.Invoke();
            }
            else
            {
                onCollidering = false;
            }


        }
    }
    

}
