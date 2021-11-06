using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortHoverState : FSMState
{
    //private Transform target;


    public MFortHoverState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Hover;


    }

    public override void Act(IEnemy enemy)
    {
        //enemy.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        enemy.Move();

    }

    public override void Reason(IEnemy enemy)
    {
        if (true)
        {
            fsm.PerformTransition(Transition.Relocated);
        }
        //当满足xx条件后，触发重定位
    }
}
