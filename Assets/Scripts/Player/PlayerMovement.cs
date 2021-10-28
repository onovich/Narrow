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

    private Transform waveTrans;

    private LayerMask obb;
    private LayerMask bullet;
    ContactFilter2D filter;
    ContactFilter2D bulletFilter;
    public int hitNumber = 0;
    Vector3 originalCameraPos;

    bool onParry = false;


    private void Start()
    {
        trans = transform;
        waveTrans = wave.transform;
        waveOriginalPos = waveTrans.localPosition;
        obb = LayerMask.GetMask("OBB");
        bullet = LayerMask.GetMask("bullet");
        filter = new ContactFilter2D
        {
            useLayerMask = true,
            //useTriggers = false,
            useTriggers = true,
            layerMask = obb,

        };
        bulletFilter = new ContactFilter2D
        {
            useLayerMask = true,
            //useTriggers = false,
            useTriggers = true,
            layerMask = bullet,

        };

        //collide = new UnityAction(onCollide);
        //collideEvent.AddListener(collide);
        originalCameraPos = Camera.main.transform.position;

    }

    //UnityAction collide;
    //UnityEvent collideEvent = new UnityEvent();

    bool onMoving = false;
    bool onCollidering = false;


    public ParticleSystem particle;

    public SpriteRenderer wave;

    IEnumerator CameraShake()
    {
        Vector3 offset = new Vector3(0.02f, 0.02f,0f);
        Camera.main.transform.position = originalCameraPos+offset;
        yield return new WaitForSeconds(.1f);
        Camera.main.transform.position = originalCameraPos;
    }


    void onCollide(GameObject collision)
    {
        Debug.Log("碰撞！");
        //rigid.velocity = new Vector3(0, 0, 0);

        //rigid.velocity = new Vector3(0, 0, 0);
        //Vector2 offset = Horizontal > 0 ? trans.right : -trans.right;
        //rigid.AddForce(-offset * 400);

        StopCoroutine(CameraShake());
        StartCoroutine(CameraShake());

        if (collision.CompareTag("npc"))
        {
            particle.Play();
            Destroy(collision);

        }
        if (collision.CompareTag("wall"))
        {
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

        

        


        

        
    }

    bool stop = true;
    Vector3 waveOriginalPos;
    float waveOffset = 1;

    public SpriteRenderer parry;



    void Rebound(BulletString bullet)
    {
        particle.Play();
        bullet.SetDir(-bullet.direction);

    }

    void ParryOn()
    {
        if (!onParry)
        {
            parry.DOColor(new Color(255, 255, 255, 255), 1f);

        }
        onParry = true;

    }
    void ParryOff()
    {
        if (onParry)
        {
            parry.DOColor(new Color(255, 255, 255, 0), .4f);

        }
        onParry = false;

    }

    void Update()
    {
        //Debug.Log("当前速度="+rigid.velocity+",Time:"+Time.time);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("护盾开启");
            ParryOn();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("护盾关闭");
            ParryOff();
        }

        //角色碰撞检测
        RaycastHit2D[] bulletHurtHits = new RaycastHit2D[36];
        RaycastHit2D[] bulletReboundHits = new RaycastHit2D[36];

        float bulletHurtDis = .01f;
        float bulletReboundDis = 1f;

        Vector2 bulletOffset = Horizontal > 0 ? trans.right : -trans.right;
        int bulletHurtHitNumber = Physics2D.Raycast(trans.position, bulletOffset * .01f, bulletFilter, bulletHurtHits, bulletHurtDis);
        int bulletReboundHitNumber = Physics2D.Raycast(trans.position, bulletOffset * .01f, bulletFilter, bulletReboundHits, bulletReboundDis);


        if ((!onParry)&&(bulletHurtHitNumber > 0))
        {
            particle.Play();
            Destroy(gameObject); 

        }

        if ((onParry)&&(bulletReboundHitNumber > 0))
        {
             
            Rebound(bulletReboundHits[0].collider.GetComponent<BulletString>());

        }



        if (Input.GetKey(KeyCode.D))
        {
            waveOffset = 1;
            waveTrans.localPosition = waveOriginalPos * waveOffset;
        }
        if (Input.GetKey(KeyCode.A))
        {
            waveOffset = -1;
            waveTrans.localPosition = waveOriginalPos * waveOffset;
        }

        waveTrans.localScale = new Vector3(waveOffset, 1/trans.localScale.y,1); 

         


        if ((Mathf.Abs(rigid.velocity.x) >= 2f)&&(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)))
        {
            //wave.DOColor(new Color(255,255,255,255),1f);
            wave.color = Color.Lerp(wave.color, new Color(255, 255, 255, 255), .01f);
        }

        else
        {
            //wave.DOColor(new Color(255, 255, 255, 0), .1f);
            wave.color = Color.Lerp(wave.color, new Color(255, 255, 255, 0), .1f);

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
            

            particle.Simulate(0, false, true);
            particle.Play();

            StopCoroutine(CameraShake());
            StartCoroutine(CameraShake());
        }

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
            hitNumber = Physics2D.Raycast(trans.position, offset *.01f, filter, hits, dis);

            if (hitNumber > 0)
            {
                onCollidering = true;
                //collideEvent.Invoke();
                onCollide(hits[0].collider.gameObject);
            }
            else
            {
                onCollidering = false;
            }


        }
    }
    

}
