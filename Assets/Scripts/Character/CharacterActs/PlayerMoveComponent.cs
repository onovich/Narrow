using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public interface IPlayerMoveComponent
{
    void Ctor(Transform trans, MoveSetting setting, TrailRenderer trail, Rigidbody2D rigid, ParticleSystem collideEffect);
    void Move();
    void SetActive();
    void SetBlock();
    int Direction { get;}
    bool OnMoving { get; set; }
    bool OnMovingController { get; set; }
}

public class PlayerMoveComponent :IPlayerMoveComponent
{
    // 外部依赖
    private float speed;
    //float trailWidth;
    float trailWidthScale;
    TrailRenderer trail;
    Rigidbody2D rigid;
    Transform trans;
    ParticleSystem collideEffect;

    // 对外字段
    [HideInInspector]
    public bool movable = true;
    public bool OnMoving { get; set; } = false;
    public bool OnMovingController { get; set; } = false;

    // 内部字段
    private Vector3 normalSpeed;//归一化的速度向量方向

    [HideInInspector]
    public int hitNumber = 0;
    bool blocked = true;

    // 属性
    float Horizontal => Input.GetAxis("Horizontal");
    //public int Direction { get { if (Horizontal > 0) return 1; else if (Horizontal < 0) return -1; else return 0; }}
    public int Direction { get; set; } = 1;


    /// 初始化+获取依赖
    public void Ctor(Transform trans, MoveSetting setting, TrailRenderer trail, Rigidbody2D rigid, ParticleSystem collideEffect)
    {
        this.trans = trans;
        this.trail = trail;
        this.speed = setting.speed;
        this.rigid = rigid;
        this.collideEffect = collideEffect;

    }


    public void SetActive()
    {
        blocked = false;
        //Debug.Log("blocked = false");
    }
    public void SetBlock()
    {
        blocked = true;
        //Debug.Log("blocked = true");
    }

    /// 逻辑层实现
    public void Move()
    {
        //Debug.Log("准备移动");
        if (movable)
        {
            if (!blocked)
            {
                //角色移动控制
                normalSpeed = (new Vector3(Horizontal, 0, 0)).normalized;
                if (normalSpeed != Vector3.zero)
                {
                    rigid.velocity += (Vector2)normalSpeed * speed * Time.smoothDeltaTime;
                }

                OnMoving = rigid.velocity == Vector2.zero ? false : true;
                OnMovingController = Horizontal == 0 ? false : true;

                if (Input.GetKey(KeyCode.A)) { Direction = -1; }
                else if (Input.GetKey(KeyCode.D)) { Direction = 1; }
                //else Direction = 0;




}


        }
        
    }
 
}
