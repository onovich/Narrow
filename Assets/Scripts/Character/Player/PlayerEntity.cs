using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerEntity
{
    int Direction { get; }
    bool OnAttack { get; }
}

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerEntity : MonoBehaviour, IPlayerEntity
{
    // 数据层
    // 引用
    public ParticleSystem hurtEffect;
    public ParticleSystem deadEffect;
    public ParticleSystem collideEffect;
    public TrailRenderer trail;
    // Setting
    public DestructibleSetting destructibleSetting;
    public MoveSetting moveSetting;
    // 预设
    public bool hurtable = false;
    public bool destroyable = false;
    public SpriteRenderer parrySprite;
    public Sprite parryWhite;
    public Sprite parryBlack;
   

    public bool OnAttack { get=>destructibleComponent.OnAttack; }


    // 属性
    public int Direction { get => playerMoveComponent.Direction; } 

    // 内部字段
    Rigidbody2D rigid;

    // 行为层组件
    IPlayerMoveComponent playerMoveComponent;
    IDestructibleComponent destructibleComponent;
    ICollideReactComponent collideReactComponent;
    IDodgeComponent dodgeComponent;
    IParryComponent parryComponent;

    // 控制层
    IPlayerControllerComponent playerControllerComponent;

    // 事件
    event OnMoveEventHandler OnMoveEvent;
    event OnMovingEventHandler OnMovingEvent;
    event OnParryOnEventHandler OnParryOnEvent;
    event OnDodgeEventHandler OnDodgeEvent;
    event OnParryDoneEventHandler OnParryDoneEvent;
    event OnParryCancelEventHandler OnParryCancelEvent;

    //--------------------------------------------------------------------------

    /// 创建组件+注入依赖
    public void Ctor()
    {

        //一级行为层
        destructibleComponent = gameObject.AddComponent<DestructibleComponent>();
        destructibleComponent.Ctor(trail, moveSetting, destructibleSetting, hurtable, destroyable, hurtEffect, deadEffect);

        playerMoveComponent = new PlayerMoveComponent();
        playerMoveComponent.Ctor(transform, moveSetting, trail,rigid, collideEffect);

        dodgeComponent = gameObject.AddComponent<DodgeComponent>();
        dodgeComponent.Ctor(this,GetComponent<BoxCollider2D>());

        parryComponent = gameObject.AddComponent<ParryComponent>();
        parryComponent.Ctor(this,GetComponent<SpriteRenderer>(), parrySprite, OnParryDoneEvent, OnParryCancelEvent, parryWhite,parryBlack);

        collideReactComponent = gameObject.AddComponent<CollideReactComponent>();
        collideReactComponent.Ctor(Direction, collideEffect,true);

        
        //订阅事件
        OnMoveEvent += playerMoveComponent.Move;
        OnMovingEvent += playerMoveComponent.SetActive;

        OnDodgeEvent += dodgeComponent.Dodge;
        OnParryOnEvent += parryComponent.ParryOn;

        OnParryDoneEvent += collideReactComponent.SetBulletDestructibleOff;
        OnParryDoneEvent += destructibleComponent.SetDestructibleOff;

        OnParryCancelEvent += collideReactComponent.SetBulletDestructibleOn;
        OnParryCancelEvent += destructibleComponent.SetDestructibleOn;

        //二级行为层
        playerControllerComponent = new PlayerControllerComponent();
        playerControllerComponent.Ctor(OnMoveEvent, OnMovingEvent, OnParryOnEvent, OnDodgeEvent);

    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Ctor();
    }

    //--------------------------------------------------------------------------


    /// 逻辑层主循环
    private void Update()
    {
        playerControllerComponent.Update();
        playerMoveComponent.HitChecking();
    }

}