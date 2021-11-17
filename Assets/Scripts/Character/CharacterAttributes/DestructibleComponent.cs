using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public interface IDestructibleComponent
{
    void GetHurt(float attackValue);
    void Destroy();
    void Ctor(DestructibleSetting setting, bool hurtable, bool destroyable, ParticleSystem hurtEffect, ParticleSystem deadEffect, OnGetHurtEventHandler OnGetHurtEvent);
    void Ctor(TrailRenderer trail, MoveSetting moveSetting,TrailSetting trailSetting, DestructibleSetting setting, bool hurtable, bool destroyable, ParticleSystem hurtEffect, ParticleSystem deadEffect, OnGetHurtEventHandler OnGetHurtEvent);
    bool OnAttack { get; set; }
    bool hurtable { get; set; }
    void SetDestructibleOn();
    void SetDestructibleOff();
}


public delegate void OnGetHurtEventHandler();


[RequireComponent(typeof(BoxCollider2D))]
public class DestructibleComponent : MonoBehaviour, IDestructibleComponent
{
    // 外部依赖
    DestructibleSetting setting;
    [Range(0, 10)]
    float maxhp;
    bool destroyable = false;
    private ParticleSystem hurtEffect;
    private ParticleSystem deadEffect;
    IPlayerEntity player;
    float trailTime;
    TrailRenderer trail;

    // 属性
    public bool OnAttack { get;set; }
    public bool hurtable { get; set; } = false;

    // 内部字段
    bool ifPlayer = false;
    float hp;
    SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    Tween tweener;
    Tween tweener2;

    //事件
    event OnGetHurtEventHandler OnGetHurtEvent;

    /// 获取依赖
    public void Ctor(DestructibleSetting setting, bool hurtable, bool destroyable, ParticleSystem hurtEffect, ParticleSystem deadEffect, OnGetHurtEventHandler OnGetHurtEvent)
    {
        this.setting = setting;
        this.hurtable = hurtable;
        this.destroyable = destroyable;
        this.hurtEffect = hurtEffect;
        this.deadEffect = deadEffect;
        this.OnGetHurtEvent = OnGetHurtEvent;

    }
    public void Ctor(TrailRenderer trail, MoveSetting moveSetting,TrailSetting trailSetting, DestructibleSetting setting, bool hurtable, bool destroyable, ParticleSystem hurtEffect, ParticleSystem deadEffect, OnGetHurtEventHandler OnGetHurtEvent)
    {
        this.trail = trail;
        this.trailTime = trailSetting.trailLifeTime;
        Ctor(setting, hurtable, destroyable, hurtEffect, deadEffect, OnGetHurtEvent);
    }

    /// 初始化
    private void Start()
    {
        ifPlayer = gameObject.CompareTag("player");
        maxhp = setting.maxHp;
        hp = maxhp;
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    /// 行为层实现
    /// 对外实现
    void RefreshTrail()
    {
        if (maxhp > 0)
        {
            trail.time = trailTime * hp / maxhp;
        }
    }
    public void GetHurt(float attackValue)
    {
        if (hurtable)
        {
            StopCoroutine(Flash());
            StartCoroutine(Flash());
            if (ifPlayer)
            {
                //Debug.Log("确实是player");
                RefreshTrail();
            }

            hp -= attackValue;
            //广播：受到伤害
            OnGetHurtEvent?.Invoke();
            //Debug.LogError("广播：受到伤害");

            if ((destroyable) && (hp <= 0))
            {
                Destroy();

            }
        }
    }
    public void SetDestructibleOn()
    {
        hurtable = true ;
    }

    public void SetDestructibleOff()
    {
        hurtable = false;
    }

    /// 内部实现
    public void Destroy()
    {
        StartCoroutine(Explore());
    }
    IEnumerator Flash()
    {
        OnAttack = true;
        if(hp>0)hurtEffect.Play();
        if (tweener == null)
        {
            tweener = sprite.DOColor(new Color(1, 1, 1, .5f), .2f);
            tweener.SetAutoKill(false);
            yield return tweener.WaitForCompletion();
        }
        else
        {
            tweener.Restart();
            yield return tweener.WaitForCompletion();
        }
        if (tweener2 == null)
        {
            tweener2 = sprite.DOColor(new Color(1, 1, 1, 1), .1f);
            tweener2.SetAutoKill(false);
            yield return tweener2.WaitForCompletion();
        }
        else
        {
            tweener2.Restart();
            yield return tweener2.WaitForCompletion();
        }
        OnAttack = false;
    }

    IEnumerator Explore()
    {
        
        deadEffect.Play();
        yield return new WaitForSeconds(.2f);
        sprite.enabled = false;
        if (ifPlayer)
        {
            trail.enabled = false;
        }

        boxCollider.enabled = false;
        yield return new WaitForSeconds(.2f);
        Destroy(this.gameObject);

    }

    
}
