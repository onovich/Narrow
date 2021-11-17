using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackFatigueComponent
{
    //bool IfFatigue();
    void Count();
    void Ctor(AttackFatigueSetting attackFatigueSetting);
    bool IfFatigue { set; get; }
}
public class AttackFatigueComponent : IAttackFatigueComponent
{
    int threshold;
    int count = 0;
    //bool ifFatigue = false;

    public void Ctor(AttackFatigueSetting setting)
    {
        this.threshold = setting.threshold;
    }

    public void Count()
    {
        count++;
        
        
        
        //Debug.Log("攻击疲劳计数:"+count+",当前疲劳状态:"+ifFatigue+",count % threshold==" + count % threshold);
    }

    /*
    public bool IfFatigue()
    {
        if(ifFatigue) Debug.LogError("疲劳,count=" + count + "threshold=" + threshold);
        return ifFatigue;
    }
    */

    public bool IfFatigue { set { } get
        {
            if ((count != 0) && (count % threshold == 0))
            {
                count = 0;
                return true;
            }
            else
            {
                return false;
            }
        } }
    






}
