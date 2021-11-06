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
        enemy.Attack();
    }

    public override void Reason(IEnemy enemy)
    {
        if (enemy.Fatigue())
        {
            enemy.AttackOff();
            fsm.PerformTransition(Transition.Fatigue);
        }
    }
}