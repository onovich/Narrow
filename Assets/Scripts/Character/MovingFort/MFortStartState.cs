using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFortStartState : FSMState
{
    //private Transform target;


    public MFortStartState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Start;
    }

    public override void Act(IEnemy enemy)
    {
        enemy.FadeIn();
    }

    public override void Reason(IEnemy enemy)
    {
        if (enemy.TimeUp())
        {
            fsm.PerformTransition(Transition.TimeUp);
        }
        Debug.Log("满足条件，切换状态为Attack");
    }
}
