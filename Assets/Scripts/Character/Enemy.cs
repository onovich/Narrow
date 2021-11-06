using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyActiveState
{
    active,
    stop,
}

public interface IEnemy
{
    EnemyActiveState ActiveState { get; set; }
    Transform LockedTarget { get; set; }
    void Stop();
    void SetActive();
    void FadeIn();
    void FadeOut();
    void Attack();
    void AttackOff();
    void Move();
    void GetHurt(float attackValue);
    void Destroy();
    void Hover();
    bool Fatigue();
    bool TimeUp();

}



