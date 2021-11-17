using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortWinddownState : FSMState
{
    //private Transform target;


    public MFortWinddownState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Winddown;

    }

    public override void Act(IEnemy enemy)
    {
        enemy.Winddown();

    }

    public override void Reason(IEnemy enemy)
    {
        if (enemy.IfWinddownDone())
        {
            fsm.PerformTransition(Transition.WinddownDone);
        }
        //当满足xx条件后，触发重定位
    }
}
