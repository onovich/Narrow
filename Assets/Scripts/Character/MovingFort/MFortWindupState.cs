using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortWindupState : FSMState
{
    //private Transform target;


    public MFortWindupState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Windup;

    }

    public override void Act(IEnemy enemy)
    {
        enemy.Windup();

    }

    public override void Reason(IEnemy enemy)
    {
        if (enemy.IfWindupDone())
        {
            fsm.PerformTransition(Transition.WindupDone);
        }
        //当满足xx条件后，触发重定位
    }
}
