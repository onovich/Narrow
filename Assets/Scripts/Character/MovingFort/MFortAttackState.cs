using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortAttackState : FSMState
{
    //private Transform target;

    public MFortAttackState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Attack;
    }

    public override void Act(IEnemy enemy)
    {
        enemy.SetDir();
        enemy.Attack();
    }

    public override void Reason(IEnemy enemy)
    {
        if (enemy.IfFatigue())
        {
            Debug.Log("攻击疲劳");
            enemy.AttackOff();
            fsm.PerformTransition(Transition.Fatigue);
        }
    }
}