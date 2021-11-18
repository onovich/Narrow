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
    int Direction { get; set; }
    void Stop();
    void SetActive();
    void FadeIn();
    void FadeOut();
    void Attack();
    void AttackOff();
    void Move();
    void GetHurt(float attackValue);
    void Destroy();
    void Sprint();
    void StopSprint();
    void Windup();
    void Winddown();
    bool IfFatigue();
    bool IfTimeUp();
    bool IfWindupDone();
    bool IfRelocated();
    bool IfWinddownDone();
    
    void SetStatic();
    void RemoveStatic();
    void SetDir();


}



