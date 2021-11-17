using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MFortFSM : IFSM
{
    public IEnemy Enemy;

    public MFortFSM(MFortEntity mFortEntity, StateID defaultState, IEnemy Enemy)
    {
        this.Enemy = Enemy;
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
        attackState.ADDTransition(Transition.Fatigue, StateID.Windup);

        //冲刺前摇的情况下
        FSMState windupState = new MFortWindupState(fsm);
        //前摇结束后，进入冲刺状态
        windupState.ADDTransition(Transition.WindupDone, StateID.Sprint);


        //冲刺的情况下
        FSMState sprintState = new MFortSprintState(fsm);
        //重定位后，进入后摇状态
        sprintState.ADDTransition(Transition.Relocated, StateID.Winddown);

        //后摇的情况下
        FSMState winddownState = new MFortWinddownState(fsm);
        //重定位后，进入后摇状态
        winddownState.ADDTransition(Transition.WinddownDone, StateID.Attack);


        //注册状态
        fsm.AddState(startState);
        fsm.AddState(attackState);
        fsm.AddState(windupState);
        fsm.AddState(sprintState);
        fsm.AddState(winddownState);

        //指定默认状态
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

 