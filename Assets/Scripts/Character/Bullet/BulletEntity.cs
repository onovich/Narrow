using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletEntity : MonoBehaviour
{
    // 外部依赖
    public GameObject trail;
    public  BulletSetting setting;

    // 内部字段
    float speed = 0;
    [Range(-1, 1)]
    private int direction = 0;
    Rigidbody2D rigid;
    Transform trans;
    SpriteRenderer sprite;
    TrailRenderer trailRenderer;
    RaycastCreater raycastCreater;
    Vector3 dustbinPlace = new Vector3(20, 20, 0);

    [HideInInspector]
    public bool ifRebound = false;

    /// 对外方法
    public void ChangeDir(int factor)
    {
        if (factor != 0)
        {
            direction *= Mathf.Clamp(factor, -1, 1);
        }
    }

    /// 获取依赖注入
    public void Ctor(int direction)
    {
        this.direction = direction;
    }

    /// ReStart初始化
    void OnEnable()
    {
        sprite.enabled = true;
        trail.SetActive(false);
        ifRebound = false;
        trail.GetComponent<TrailRenderer>().Clear();
    }

    /// 初始化+获取依赖
    void Awake()
    {
        speed = setting.speed;
        trans = transform;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trailRenderer = trail.GetComponent<TrailRenderer>();
        raycastCreater = new RaycastCreater(trans);
    }

    
    /// 内部方法
    /// 碰撞检测
    void Hit(GameObject hit)
    {
        Debug.Log("击中" + hit.name);
        hit.GetComponent<DestructibleComponent>().GetHurt(GetComponent<AttackComponent>().attackValue);
        CameraShake.instance.Shake();
        sprite.enabled = false;
        trans.position = dustbinPlace;
        trailRenderer.Clear();
     }

    /// 主循环
    /// 运动层
    private void FixedUpdate()
    {
        if ((direction != 0) && (speed != 0))
        {
            Vector3 normalSpeed;
            normalSpeed = (new Vector3(direction, 0, 0)).normalized * speed;
            rigid.velocity += (Vector2)normalSpeed * speed * Time.deltaTime;
        }
    }
    /// 逻辑层
    void Update()
    {

        float dis = .12f;
        GameObject hit = raycastCreater.Raycast("harmful",5, direction,dis,false);//ignoreSelf选false是因为考虑到会被发射出的子弹再反弹后伤害

        if (hit != null)
        {
            DestructibleComponent destructibleComponent = hit.GetComponent<DestructibleComponent>();
            ShootComponent fort = hit.GetComponent<ShootComponent>();
            if (fort != null)
            {
                if (ifRebound) Hit(hit);
            }
            else if (destructibleComponent.hurtable) Hit(hit);
        }
        else
        {
            //Debug.Log("未击中");
        }
    }
}
