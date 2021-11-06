using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using deVoid.Utils;
using System;


public interface IPlayerControllerComponent
{
    void Ctor(OnMoveEventHandler OnMoveEvent, OnMovingEventHandler OnMovingEvent,OnParryOnEventHandler OnParryOnEvent, OnDodgeEventHandler OnDodgeEvent);
    void Update();
}

public delegate void OnMoveEventHandler();
public delegate void OnMovingEventHandler();
public delegate void OnParryOnEventHandler();
public delegate void OnDodgeEventHandler();

public class PlayerControllerComponent : IPlayerControllerComponent
{

    event OnMoveEventHandler OnMoveEvent;
    event OnMovingEventHandler OnMovingEvent;
    event OnParryOnEventHandler OnParryOnEvent;
    event OnDodgeEventHandler OnDodgeEvent;

    /* 在别处订阅
     * npcTargetAttribute.OnNpcMoveEvent += Refresh;
     * 发出广播:
     * OnMove?.Invoke();
     */


    public void Ctor(OnMoveEventHandler OnMoveEvent, OnMovingEventHandler OnMovingEvent,OnParryOnEventHandler OnParryOnEvent, OnDodgeEventHandler OnDodgeEvent)
    {
        this.OnMoveEvent = OnMoveEvent;
        this.OnMovingEvent = OnMovingEvent;
        this.OnParryOnEvent = OnParryOnEvent;
        this.OnDodgeEvent = OnDodgeEvent;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand))
        {
            OnDodgeEvent?.Invoke();
            Debug.Log("获取闪避控制");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnParryOnEvent?.Invoke();
            Debug.Log("获取护盾控制");
        }
        if (Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
        {
            OnMoveEvent?.Invoke();
            Debug.Log("获取移动中控制");
        }
        if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D)))
        {
            OnMovingEvent?.Invoke();
            Debug.Log("获取移动控制");
        }
    }


}
