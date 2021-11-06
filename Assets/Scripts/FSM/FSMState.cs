using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Transition
{
    NullTransition = 0,

    TimeUp,//计时完成
    SeePlayer,//看见玩家
    LostPlayer,//跟丢玩家
    Fatigue,//疲劳
    Relocated,//完成重定位

}


public enum StateID
{
    NullStateID = 0,

    Start,//初始化
    Pend,//待机
    Attack,//攻击
    Hover,//徘徊
}
public abstract class FSMState 
{
    protected StateID stateID;
    protected FSMSystem fsm;

    public StateID ID
    {
        get { return stateID; }

    }

    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

    public FSMState(FSMSystem fsm)
    {
        this.fsm = fsm;
    }


    public void ADDTransition(Transition transition, StateID id)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("不允许添加NullTransition");
            return;
        }
        if (id == StateID.NullStateID)
        {
            Debug.LogError("不允许添加NullStateID");
            return;
        }
        if (map.ContainsKey(transition))
        {
            Debug.LogError("已经存在transition");
            return;
        }
        map.Add(transition, id);
    }
    public void DeleteTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("不允许删除NullTransition");
            return;
        }
        if (!map.ContainsKey(transition))
        {
            Debug.LogError("transition不存在");
            return;
        }
        map.Remove(transition);

    }
    public StateID GetStateID(Transition transition)
    {
        if (map.ContainsKey(transition))
        {
            return map[transition];
        }
        return StateID.NullStateID;
    }





    public virtual void DoBeforeEntering() { }

    public virtual void DoAfterLeaving() { }

    public abstract void Act(IEnemy enemy);

    public abstract void Reason(IEnemy enemy);





}
