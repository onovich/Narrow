using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortSprintState : FSMState
{
    //private Transform target;


    public MFortSprintState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Sprint;
    }

    public override void Act(IEnemy enemy)
    {
        enemy.Sprint();

    }

    public override void Reason(IEnemy enemy)
    {
        if (enemy.IfRelocated())
        {
            enemy.StopSprint();
            Debug.LogError("完成徘徊，切换状态");
            fsm.PerformTransition(Transition.Relocated);
        }
        //当满足xx条件后，触发重定位
    }
}
