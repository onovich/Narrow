using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMFortSprintComponent
{
    void Sprint();
    void Ctor(Transform trans, MFortEntity mFort, Rigidbody2D rigid, Transform transA, Transform transB, IDestructibleComponent destructible);
    //bool IfSprintDone();
    bool IfSprintDone { get; set; }
    void StopSprint();
    void SetStatic();
    void RemoveStatic();
    void SetSprintLayer();
    void ResetLayer();
}

public class MFortSprintComponent : MonoBehaviour, IMFortSprintComponent
{
    RaycastCreater raycastCreater;
    Transform trans;
    MFortEntity mFort;
    Rigidbody2D rigid;
    //Collider2D collider2d;
    Transform transA;
    Transform transB;
    IDestructibleComponent destructible;
    //bool OnAttack = false;
    bool isOnHover = false;


    void SprintHurt(GameObject hit)
    {
        if (hit.GetComponent<DestructibleComponent>() == null) Debug.LogError("空对象DestructibleComponent");
        if (GetComponent<AttackComponent>() == null) Debug.LogError("空对象AttackComponent");
        hit.GetComponent<DestructibleComponent>().GetHurt(GetComponent<AttackComponent>().attackValue);
        CameraShake.instance.Shake();
    }

    public void SetSprintLayer()
    {
        trans.gameObject.layer = LayerMask.NameToLayer("sprint");
    }
    public void ResetLayer()
    {
        trans.gameObject.layer = LayerMask.NameToLayer("harmful");
    }

    public void SetStatic()
    {
        rigid.bodyType = RigidbodyType2D.Static;
    }

    public void RemoveStatic()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    
    bool Hit()
    {
        float dis = .5f;
        GameObject hit = raycastCreater.Raycast(5, mFort.direction, dis, true);
        if (hit != null)
        {
            if(hit.gameObject.layer==(LayerMask.NameToLayer("harmful")))
            {
                Debug.Log("击中且击中可伤害物体");
                SprintHurt(hit);
                return false;
            }
            else
            {
                Debug.Log("击中不可穿透物体");
                return true;
            }
        }
        else
        {
            Debug.Log("未击中");
            //IfSprintDone = false;
            return false;
        }
    }
    
    
    public void Ctor(Transform trans,MFortEntity mFort,Rigidbody2D rigid,Transform transA,Transform transB, IDestructibleComponent destructible)
    {
        this.trans = transform;
        this.mFort = mFort;
        this.rigid = rigid;
        //this.collider2d = collider;
        this.transA = transA;
        this.transB = transB;
        this.destructible = destructible;
        raycastCreater = new RaycastCreater(trans);
        //raycastCreater.ifShowsLog = true;
    }

     

    public void Hover()
    {
        if (!isOnHover)
        {
            isOnHover = true;
            StartCoroutine(Hovering());
        }
    }

    Vector2 searchNextPos()
    {
        float disA = Vector2.Distance(trans.position, transA.position);
        float disB = Vector2.Distance(trans.position, transB.position);
        Vector2 result = disA > disB ? transA.position : transB.position;
        return result;
    }

    void SetDirToTarget(Vector2 target)
    {
        int direction = target.x > trans.position.x ? 1 : -1;
        mFort.direction = direction;
    }
    

    IEnumerator Hovering()
    {
        SetSprintLayer();
        IfSprintDone = false;
        int times = Random.Range(1, 3);
        float speed = .10f;

        for (int i = 0; i < times; i++)
        {
            Vector2 target = searchNextPos();

            int direction = target.x > trans.position.x ? 1 : -1;
            SetDirToTarget(target);

            Vector3 normalSpeed;
            normalSpeed = (new Vector3(direction, 0, 0)).normalized;
            float dis = Vector2.Distance(trans.position, target);

            while (dis > .05)
            {
                if (Hit())
                {
                    //Debug.LogError("触碰退出循环");
                    break;
                }
                else
                {
                    //rigid.AddForce((Vector2)normalSpeed * speed * 4000 * Time.smoothDeltaTime);
                    rigid.MovePosition((Vector2)trans.position+new Vector2( direction* speed,0));
                    //trans.position = Vector3.MoveTowards();
                    dis = Vector2.Distance(trans.position, target);
                    //Debug.Log("dis="+dis);
                    yield return null;
                }
                
                
            }
            //Debug.LogError("hover循环完成，当前迭代"+i+"，总计迭代"+times);
            //yield return new WaitForSeconds(.5f);
        }
        IfSprintDone = true;
        ResetLayer();

    }



     
    

    public void Sprint()
    {
        Hit();
         Hover();
        //Debug.Log("执行徘徊动作");
    }
    public void StopSprint()
    {
        SetDir();
        isOnHover = false;

        //rigid.velocity = Vector2.zero;
    }
    public void SetDir()
    {
        mFort.direction = Global.instance.player.transform.position.x > transform.position.x ? 1 : -1;
    }

    public bool IfSprintDone { get; set; }

}
