using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollideReactComponent
{
    bool DestroyBullet { get; set; }
    void Ctor(int direction, ParticleSystem collideEffect,bool DestroyBullet);
    void SetBulletDestructibleOn();
    void SetBulletDestructibleOff();
}

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class CollideReactComponent : MonoBehaviour, ICollideReactComponent
{
    // 外部依赖
    ParticleSystem collideEffect;
    [Range(0,1)]
    int direction = 1;

    // 对外属性
    bool _destoryBullet = true;
    public bool DestroyBullet { get => _destoryBullet; set => _destoryBullet = value; }


    /// 获取依赖
    public void Ctor(int direction, ParticleSystem collideEffect,bool DestroyBullet)
    {
        this.direction = direction;
        this.collideEffect = collideEffect;
    }


    /// 行为层实现
    /// 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DestroyBullet)
        {
            if (collision.gameObject.CompareTag("bullet"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        
        if (collideEffect!=null)
        {
            collideEffect.Play();
        }
        CameraShake.instance.Shake();
    }
    /// 修改是否消除子弹
    public void SetBulletDestructibleOn()
    {
        DestroyBullet = true;
    }
    public void SetBulletDestructibleOff()
    {
        DestroyBullet = false;
    }
}
