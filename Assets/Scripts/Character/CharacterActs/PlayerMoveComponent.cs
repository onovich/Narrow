using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public interface IPlayerMoveComponent
{
    void Ctor(Transform trans, MoveSetting setting, TrailRenderer trail, Rigidbody2D rigid, ParticleSystem collideEffect);
    void Move();
    void SetActive();
    int Direction { get;}
    void HitChecking();
}

public class PlayerMoveComponent :IPlayerMoveComponent
{
    // 外部依赖
    private float speed;
    float trailWidth;
    float trailWidthScale;
    TrailRenderer trail;
    Rigidbody2D rigid;
    Transform trans;
    ParticleSystem collideEffect;


    [HideInInspector]
    public bool movable = true;

    // 内部字段
    private Vector3 normalSpeed;//归一化的速度向量方向
    private Vector3 speedUpScale;
    RaycastCreater raycastCreater;

    [HideInInspector]
    public int hitNumber = 0;
    bool stop = true;
    bool onMoving = false;
    bool onCollidering = false;

    // 属性
    float Horizontal => Input.GetAxis("Horizontal");
    public int Direction { get { if (Horizontal > 0) return 1; else if (Horizontal <= 0) return -1; else return 0; }}



    /// 初始化+获取依赖
    public void Ctor(Transform trans, MoveSetting setting, TrailRenderer trail, Rigidbody2D rigid, ParticleSystem collideEffect)
    {
        this.trans = trans;
        this.trail = trail;
        this.trailWidth = setting.trailWidth;
        this.trailWidthScale = setting.trailWidthScale;
        this.speed = setting.speed;
        this.rigid = rigid;
        this.collideEffect = collideEffect;
        this.speedUpScale = (Vector3)setting.speedUpScale + Vector3.forward;
        raycastCreater = new RaycastCreater(trans);

    }



    /// 行为层实现
    /// 碰撞实现
    void onCollide(GameObject collision)
    {
        CameraShake.instance.Shake();

        if (collision.CompareTag("npc"))
        {
            collideEffect.Play();
            Object.Destroy(collision);

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

    public void SetActive()
    {
        stop = false;
    }

    /// 逻辑层实现
    public void Move()
    {
        Debug.Log("准备移动");
        if (movable)
        {
            if (!stop)
            {
                //角色移动控制
                normalSpeed = (new Vector3(Horizontal, 0, 0)).normalized;
                if (normalSpeed != Vector3.zero)
                {
                    rigid.velocity += (Vector2)normalSpeed * speed * Time.smoothDeltaTime;
                }
            }

            //形变控制
            //移动时:未因碰撞停止且未碰撞，且有按加速键
            if (((!stop) && (!onCollidering)) && (Horizontal != 0))
            {
                //缩小拉长
                onMoving = true;
                //除重
                if (trans.localScale != speedUpScale)
                {
                    trans.localScale = Vector3.Lerp(trans.localScale, speedUpScale, .1f);
                    trail.startWidth = Mathf.Lerp(trail.startWidth, trailWidthScale, .1f);
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
                    trans.localScale = Vector3.Lerp(trans.localScale, Vector3.one, .1f);
                    trail.startWidth = Mathf.Lerp(trail.startWidth, trailWidth, .1f);
                }
            }

        }
        else
        {
            Debug.Log("不可移动");
        }
    }

    /// 主循环
    public void HitChecking()
    {
        //碰撞检测
        float dis = .1f;
        GameObject hit = raycastCreater.Raycast("OBB", 3, Direction, dis, true);

        if (hit != null)
        {
            onCollidering = true;
            onCollide(hit);
        }
        else
        {
            onCollidering = false;
        }
    }
}
