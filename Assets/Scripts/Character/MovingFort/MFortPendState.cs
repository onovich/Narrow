using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortPendState : FSMState
{

    //private Transform target;


    public MFortPendState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Pend;


    }

    public override void Act(IEnemy enemy)
    {
        //enemy.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        //enemy.GetComponent<MovingFortEntitie>().Move();

    }

    public override void Reason(IEnemy enemy)
    {
        if (true)
        {
            //fsm.PerformTransition(Transition.Reset);
        }
        //当满足xx条件后，触发重定位
    }
}
