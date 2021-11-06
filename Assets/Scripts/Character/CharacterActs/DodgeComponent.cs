using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public interface IDodgeComponent
{
    void Ctor(PlayerEntity playerEntity, BoxCollider2D boxCollider);
    void Dodge();
}

public class DodgeComponent : MonoBehaviour, IDodgeComponent
{
    // 外部依赖
    Transform trans;
    PlayerEntity playerEntity;

    // 内部字段
    RaycastCreater raycastCreater;
    public float length = 2f;
    private float offset;
    public float speed = .1f;

    // 属性
    int direction { get => playerEntity.Direction; }

    //--------------------------------------------------------------------------

    /// 初始化+获取依赖
    public void Ctor(PlayerEntity playerEntity, BoxCollider2D boxCollider)
    {
        this.playerEntity = playerEntity;
        this.offset = boxCollider.size.x;
    }
    void Start()
    {
        raycastCreater = new RaycastCreater(transform);
        trans = transform;
    }

    //--------------------------------------------------------------------------

    /// 行为层实现
    /// 外部调用
    public void Dodge()
    {
        if (direction != 0)
        {
            StartCoroutine(Deliver(direction));
            Debug.Log("传送完成");
        }
    }

    /// 内部实现
    Vector3 targetTest(int direction)
    {
        Vector3 target;
        Vector3 diPos = direction > 0 ? Vector3.right : -Vector3.right;
        GameObject obbTest = raycastCreater.OverlapCirclecast(4,length, offset, true);
        if(obbTest == null)
        {
            Debug.Log("传送：畅通无阻");
            target = transform.position + diPos * length;
        }
        else
        {
            Debug.Log("传送：遇到障碍");
            float offsetX = obbTest.GetComponent<BoxCollider2D>().size.x * obbTest.transform.localScale.x ;
            target = obbTest.transform.position - diPos * offsetX;
        }
        return target;
    }
    IEnumerator Deliver(int direction)
    {
        //trail.enabled = true;
        yield return new WaitForSecondsRealtime(.1f);
        trans.position = targetTest(direction);
        yield return new WaitForSecondsRealtime(.1f);
        //trail.enabled = false;
    }

   
}
