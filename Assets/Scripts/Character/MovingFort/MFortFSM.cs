using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MFortFSM : IFSM
{
    public MFortFSM(MFortEntity mFortEntity, StateID defaultState)
    {
        Ctor(mFortEntity, defaultState);
    }

    //状态机
    protected FSMSystem fsm;

    //状态指针
    public StateID currentState { get => mFortEntity.currentState; set => mFortEntity.currentState = value; }

    MFortEntity mFortEntity;


    /// 注册状态机
    protected void Ctor(MFortEntity mFortEntity ,StateID defaultState)
    {
        //Debug.Log("Zombie初始化状态机" + gameObject.name);
        fsm = new FSMSystem();

        //初始化的情况下
        FSMState startState = new MFortStartState(fsm);
        //计时完成，进入攻击状态
        startState.ADDTransition(Transition.TimeUp, StateID.Attack);

        //攻击的情况下
        FSMState attackState = new MFortAttackState(fsm);
        //疲劳后，进入徘徊状态
        attackState.ADDTransition(Transition.Fatigue, StateID.Hover);

        //徘徊的情况下
        FSMState moveState = new MFortHoverState(fsm);
        //重定位后，进入攻击状态
        moveState.ADDTransition(Transition.Relocated, StateID.Attack);

        

        //注册状态
        fsm.AddState(moveState);
        fsm.AddState(attackState);

        //fsm.currentFSMState = defaultState == StateID.Attack ? attackState : moveState;
        fsm.currentFSMState = startState;
        this.mFortEntity = mFortEntity;
    }

    /// 主循环
    public void Update(IEnemy enemy)
    {
        fsm.Update(enemy);
        currentState = fsm.currentFSMState.ID;
    }

    
}

 