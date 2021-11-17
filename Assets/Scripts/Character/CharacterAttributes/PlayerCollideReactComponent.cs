using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPlayerCollideEventHandler();

public interface IPlayerCollideReactComponent
{
    void HitChecking();
    void Ctor(Transform trans, PlayerEntity playerEntity, TrailRenderer trail, OnPlayerCollideEventHandler OnPlayerCollide, ParticleSystem collideEffect);
}

public class PlayerCollideReactComponent : MonoBehaviour, IPlayerCollideReactComponent
{
    event OnPlayerCollideEventHandler OnPlayerCollideEvent;
    RaycastCreater raycastCreater;
    Transform trans;
    PlayerEntity playerEntity;
    ParticleSystem collideEffect;
    TrailRenderer trail;
    bool onMovingController;

    public void Ctor(Transform trans,PlayerEntity playerEntity,TrailRenderer trail, OnPlayerCollideEventHandler OnPlayerCollideEvent,ParticleSystem collideEffect)
    {
        this.trans = trans;
        this.playerEntity = playerEntity;
        this.OnPlayerCollideEvent = OnPlayerCollideEvent;
        this.collideEffect = collideEffect;
        this.trail = trail;

        raycastCreater = new RaycastCreater(trans);
    }

    public void HitChecking()
    {
        //碰撞检测
        float dis = .1f;
        GameObject hit = raycastCreater.Raycast("OBB", 3, playerEntity.Direction, dis, true);
        if (hit != null)
        {
            //Debug.LogError("碰撞");
            OnCollide(hit);
        }
        else
        {
            //Debug.Log("无碰撞,此时direction="+playerEntity.Direction);
        }
    }

    void OnCollide(GameObject collision)
    {
        CameraShake.instance.Shake();

        if (collision.CompareTag("npc"))
        {
            collideEffect.Play();
            Object.Destroy(collision);

        }
        if (collision.CompareTag("wall"))
        {
            trail.Clear();

        }
        if (playerEntity.OnMoving || playerEntity.OnMovingController)
        {
            OnPlayerCollideEvent?.Invoke();
            Debug.Log("广播：player碰撞");

        }
    }









}
