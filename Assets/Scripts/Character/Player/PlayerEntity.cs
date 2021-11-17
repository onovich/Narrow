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
    public TrailSetting trailSetting;
    // 预设
    public bool hurtable = false;
    public bool destroyable = false;
    public SpriteRenderer parrySprite;
    public Sprite parryWhite;
    public Sprite parryBlack;
   

    public bool OnAttack { get=>destructibleComponent.OnAttack; }
    public bool OnMoving { get => playerMoveComponent.OnMoving; }
    public bool OnMovingController { get => playerMoveComponent.OnMovingController; }

     

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
    IPhysicalMorphComponent physicalMorphComponent;
    IPlayerCollideReactComponent playerCollideReactComponent;

    // 控制层
    IPlayerControllerComponent playerControllerComponent;

    // 事件
    event OnMoveControllerEventHandler OnMoveControllerEvent;
    event OnMovingControllerEventHandler OnMovingControllerEvent;
    event OnParryOnControllerEventHandler OnParryOnControllerEvent;
    event OnDodgeControllerEventHandler OnDodgeControllerEvent;
    event OnParryDoneEventHandler OnParryDoneEvent;
    event OnParryCancelEventHandler OnParryCancelEvent;
    event OnStopMoveControllerEventHandler OnStopMoveControllerEvent;
    event OnPlayerCollideEventHandler OnPlayerCollideEvent;
    event OnGetHurtEventHandler OnGetHurtEvent;

    //--------------------------------------------------------------------------

    /// 创建组件+注入依赖
    public void Ctor()
    {

        // 一级行为层
        destructibleComponent = gameObject.AddComponent<DestructibleComponent>();
        destructibleComponent.Ctor(trail, moveSetting, trailSetting,destructibleSetting, hurtable, destroyable, hurtEffect, deadEffect,OnGetHurtEvent);

        playerMoveComponent = new PlayerMoveComponent();
        playerMoveComponent.Ctor(transform, moveSetting, trail,rigid, collideEffect);

        dodgeComponent = gameObject.AddComponent<DodgeComponent>();
        dodgeComponent.Ctor(this,GetComponent<BoxCollider2D>());

        parryComponent = gameObject.AddComponent<ParryComponent>();
        parryComponent.Ctor(this,GetComponent<SpriteRenderer>(), parrySprite, OnParryDoneEvent, OnParryCancelEvent, parryWhite,parryBlack);

        collideReactComponent = gameObject.AddComponent<CollideReactComponent>();
        collideReactComponent.Ctor(Direction, collideEffect,true);

        physicalMorphComponent = gameObject.AddComponent<PhysicalMorphComponent>();
        physicalMorphComponent.Ctor(transform,trail,trailSetting);

        


        // 订阅事件
        OnMovingControllerEvent += playerMoveComponent.Move;
        OnMoveControllerEvent += playerMoveComponent.SetActive;
        OnPlayerCollideEvent += playerMoveComponent.SetBlock;

        OnMovingControllerEvent += physicalMorphComponent.MoveMorph;
        OnStopMoveControllerEvent += physicalMorphComponent.StopMorph;

        OnDodgeControllerEvent += dodgeComponent.Dodge;
        OnParryOnControllerEvent += parryComponent.ParryOn;

        OnParryDoneEvent += collideReactComponent.SetBulletDestructibleOff;
        OnParryDoneEvent += destructibleComponent.SetDestructibleOff;

        OnParryCancelEvent += collideReactComponent.SetBulletDestructibleOn;
        OnParryCancelEvent += destructibleComponent.SetDestructibleOn;

        



        // 二级行为层
        playerControllerComponent = new PlayerControllerComponent();
        playerControllerComponent.Ctor(OnMoveControllerEvent, OnMovingControllerEvent, OnParryOnControllerEvent, OnDodgeControllerEvent, OnStopMoveControllerEvent);

        playerCollideReactComponent = gameObject.AddComponent<PlayerCollideReactComponent>();
        playerCollideReactComponent.Ctor(transform, this, trail, OnPlayerCollideEvent, collideEffect);
    }

    /// 注销事件
    private void OnDestroy()
    {
        OnMovingControllerEvent -= playerMoveComponent.Move;
        OnMoveControllerEvent -= playerMoveComponent.SetActive;

        OnMovingControllerEvent -= physicalMorphComponent.MoveMorph;
        OnStopMoveControllerEvent -= physicalMorphComponent.StopMorph;

        OnDodgeControllerEvent -= dodgeComponent.Dodge;
        OnParryOnControllerEvent -= parryComponent.ParryOn;

        OnParryDoneEvent -= collideReactComponent.SetBulletDestructibleOff;
        OnParryDoneEvent -= destructibleComponent.SetDestructibleOff;

        OnParryCancelEvent -= collideReactComponent.SetBulletDestructibleOn;
        OnParryCancelEvent -= destructibleComponent.SetDestructibleOn;

        OnPlayerCollideEvent -= playerMoveComponent.SetBlock;

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
        playerCollideReactComponent.HitChecking();
    }

}