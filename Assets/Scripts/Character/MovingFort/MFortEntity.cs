using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class MFortEntity : MonoBehaviour, IEnemy
{
    // 数据层
    // 外部依赖
    public DestructibleSetting destructibleSetting;
    public MoveSetting moveSetting;
    public FadeInSetting fadeInSetting;
    public AttackFatigueSetting attackFatigueSetting;
    public Transform bulletTrans;
    public ParticleSystem hurtEffect;
    public ParticleSystem deadEffect;
    public ParticleSystem collideEffect;
    [Range(-1,1)]
    public int direction;
    public bool hurtable = false;
    public bool destroyable = false;
    public Transform transA;
    public Transform transB;

    Rigidbody2D rigid;
    Collider2D collider2d;

    // 属性
    public FadeState defaultFadeState = FadeState.beenOut;
    public EnemyActiveState ActiveState { get; set; }
    public Transform LockedTarget { get; set; }
    public int Direction { get { return direction; } set { direction = value; } }

    // 行为层组件
    public ICollideReactComponent collideReactComponent;
    public IShootComponent shootComponent;
    public IFadeInComponent fadeInComponent;
    public IMoveComponent moveComponent;
    public IDestructibleComponent destructibleComponent;
    public IMFortSprintComponent sprintComponent;
    public IParryComponent parryComponent;
    public IAttackFatigueComponent attackFatigueComponent;
    public IAttackComponent attackComponent;

    // 逻辑层
    IFSM mFortFSM;
    public StateID defaultState = StateID.Attack;
    public StateID currentState = StateID.Attack;//状态指针

    // 事件
    event OnGetHurtEventHandler OnGetHurtEvent;

    //--------------------------------------------------------------------------

    /// 创建组件+注入依赖
    public void Ctor()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        attackComponent = gameObject.AddComponent<AttackComponent>();

        //零级行为层
        collideReactComponent = gameObject.AddComponent<CollideReactComponent>();
        collideReactComponent.Ctor(direction,collideEffect,true);

        //一级行为层
        shootComponent = gameObject.AddComponent<ShootComponent>();
        shootComponent.Ctor(bulletTrans, this);

        fadeInComponent = gameObject.AddComponent<FadeInComponent>();
        fadeInComponent.Ctor(transform,fadeInSetting, defaultFadeState);

        moveComponent = gameObject.AddComponent<MFortMoveComponent>();
        moveComponent.Ctor();

        

        attackFatigueComponent = new AttackFatigueComponent();
        attackFatigueComponent.Ctor(attackFatigueSetting);

        

        //逻辑层
        defaultState = StateID.Start;
        mFortFSM = new MFortFSM(this,defaultState,this);
        //this.currentState = mFortFSM.currentState;

        // 订阅事件
        OnGetHurtEvent += attackFatigueComponent.Count;

        destructibleComponent = gameObject.AddComponent<DestructibleComponent>();
        destructibleComponent.Ctor(destructibleSetting, hurtable, destroyable, hurtEffect, deadEffect, OnGetHurtEvent);

        //二级行为层
        sprintComponent = gameObject.AddComponent<MFortSprintComponent>();
        sprintComponent.Ctor(transform, this, rigid, transA, transB, destructibleComponent);

    }

    private void Awake()
    {
        Ctor();
    }
    private void Start()
    {
        SetDir();
    }
    //--------------------------------------------------------------------------

    /// 行为层实现
    /// 一级行为层
    public void SetActive()
    {
        ActiveState = EnemyActiveState.active;
    }
    public void Stop()
    {
        ActiveState = EnemyActiveState.stop;
    }
    public void FadeIn()
    {
        fadeInComponent.FadeIn();
    }
    public void FadeOut()
    {
        fadeInComponent.FadeOut();
    }
    
    public void Move()
    {
        moveComponent.Move();
    }
    
    public void GetHurt(float attackValue)
    {
        destructibleComponent.GetHurt(attackValue);
    }
    public void Destroy()
    {
        destructibleComponent.Destroy();
    }



    public void SetStatic()
    {
        sprintComponent.SetStatic();
    }

    public void RemoveStatic()
    {
        sprintComponent.RemoveStatic();
    }



    /// 二级行为层
    public void Attack()
    {
        shootComponent.Attack();
    }
    public void AttackOff()
    {
        shootComponent.AttackOff();
    }
    public void Windup()
    {
        //SetTrigger();
        fadeInComponent.FadeOut(.1f);
    }
    public void Sprint()
    {
        sprintComponent.Sprint();
    }
    public void StopSprint()
    {
        sprintComponent.StopSprint();
    }
    public void Winddown()
    {
        //RemoveTrigger();
        fadeInComponent.FadeIn();
    }

    public bool IfFadedIn()
    {
        return fadeInComponent.IfFadedIn();
    }
    public bool IfFadedOut()
    {
        return fadeInComponent.IfFadedOut();
    }
    public void SetDir()
    {
        //direction = Global.instance.player.GetComponent<PlayerEntity>().Direction;
        //direction = (int)(Mathf.Abs(Global.instance.player.transform.position.x - transform.position.x) / (Global.instance.player.transform.position.x - transform.position.x));
        direction = Global.instance.player.transform.position.x > transform.position.x ? 1 : -1;
    }

    //--------------------------------------------------------------------------
    /// 逻辑层实现
    public bool IfTimeUp()
    {
        return IfFadedIn();
    }
    public bool IfFatigue()
    {
        return attackFatigueComponent.IfFatigue;
    }
    public bool IfWindupDone()
    {
        return IfFadedOut();
    }

    public bool IfRelocated()
    {
        return sprintComponent.IfSprintDone;
    }

    public bool IfWinddownDone()
    {
        return IfFadedIn();
    }




    /// 逻辑层主循环
    public void Update()
    {
        //状态机更新
        mFortFSM.Update(this);
        Debug.Log("当前状态"+currentState);


    }

   
}
