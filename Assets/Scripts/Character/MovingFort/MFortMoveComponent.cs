using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMoveComponent
{
    void Move();
    void Ctor();
}

public class MFortMoveComponent : MonoBehaviour,IMoveComponent
{
    /// 初始化+获取依赖
    public void Ctor()
    {

    }

    /// 行为层实现
    /// 对外方法
    public void Move()
    {
    }
    /// 内部实现

}
