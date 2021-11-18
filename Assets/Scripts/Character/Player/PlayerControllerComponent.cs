using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using deVoid.Utils;
using System;


public interface IPlayerControllerComponent
{
    void Ctor(OnMoveControllerEventHandler OnMoveControllerEvent, OnMovingControllerEventHandler OnMovingControllerEvent, OnParryOnControllerEventHandler OnParryOnControllerEvent, OnDodgeControllerEventHandler OnDodgeControllerEvent, OnStopMoveControllerEventHandler OnStopMoveControllerEvent);
    void Update();
}

public delegate void OnMoveControllerEventHandler();
public delegate void OnMovingControllerEventHandler();
public delegate void OnParryOnControllerEventHandler();
public delegate void OnDodgeControllerEventHandler();
public delegate void OnStopMoveControllerEventHandler();

public class PlayerControllerComponent : IPlayerControllerComponent
{

    event OnMoveControllerEventHandler OnMoveControllerEvent;
    event OnMovingControllerEventHandler OnMovingControllerEvent;
    event OnParryOnControllerEventHandler OnParryOnControllerEvent;
    event OnDodgeControllerEventHandler OnDodgeControllerEvent;
    event OnStopMoveControllerEventHandler OnStopMoveControllerEvent;

    public void Ctor(OnMoveControllerEventHandler OnMoveControllerEvent, OnMovingControllerEventHandler OnMovingControllerEvent, OnParryOnControllerEventHandler OnParryOnControllerEvent, OnDodgeControllerEventHandler OnDodgeControllerEvent, OnStopMoveControllerEventHandler OnStopMoveControllerEvent)
    {
        this.OnMoveControllerEvent = OnMoveControllerEvent;
        this.OnMovingControllerEvent = OnMovingControllerEvent;
        this.OnParryOnControllerEvent = OnParryOnControllerEvent;
        this.OnDodgeControllerEvent = OnDodgeControllerEvent;
        this.OnStopMoveControllerEvent = OnStopMoveControllerEvent;
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftCommand)&&((Input.GetKey(KeyCode.A))||(Input.GetKey(KeyCode.D)))))
        {
            OnDodgeControllerEvent?.Invoke();
            Debug.Log("获取闪避控制");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnParryOnControllerEvent?.Invoke();
            //Debug.Log("获取护盾控制");
        }
        if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D))
        {
            OnMoveControllerEvent?.Invoke();
            //Debug.Log("获取移动中控制");
        }
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
            OnMovingControllerEvent?.Invoke();
            //Debug.Log("获取移动控制");
        }
        if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.D)))
        {
            OnStopMoveControllerEvent?.Invoke();
            // Debug.Log("获取移动停止输入控制");
        }
    }


}
