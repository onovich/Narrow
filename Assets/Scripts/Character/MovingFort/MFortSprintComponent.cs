using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMFortSprintComponent
{
    void Sprint();
    void Ctor(Transform trans, MFortEntity mFort, Rigidbody2D rigid, Collider2D collider);
    //bool IfSprintDone();
    bool IfSprintDone { get; set; }
    void StopSprint();
    void SetTrigger();
    void RemoveTrigger();
}

public class MFortSprintComponent : MonoBehaviour, IMFortSprintComponent
{
    RaycastCreater raycastCreater;
    Transform trans;
    MFortEntity mFort;
    Rigidbody2D rigid;
    Collider2D collider2d;
    
    /// 碰撞检测
    void SprintHurt(GameObject hit)
    {
        //Debug.Log("击中" + hit.name);
        if (hit.GetComponent<DestructibleComponent>() == null) Debug.LogError("空对象DestructibleComponent");
        if (GetComponent<AttackComponent>() == null) Debug.LogError("空对象AttackComponent");
        hit.GetComponent<DestructibleComponent>().GetHurt(GetComponent<AttackComponent>().attackValue);
        CameraShake.instance.Shake();
    }

    bool OnAttack = false;


    public void SetTrigger()
    {
        collider2d.isTrigger = true;
        rigid.bodyType =RigidbodyType2D.Dynamic;
    }

    public void RemoveTrigger()
    {
        collider2d.isTrigger = false;
        rigid.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collider2d.gameObject.layer == (LayerMask.NameToLayer("harmful")))
        {
            if (!OnAttack)
            {
                //发送伤害广播
            }
            OnAttack = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collider2d.gameObject.layer == (LayerMask.NameToLayer("harmful")))
        {
            OnAttack = false;
        }
    }







    bool Hit()
    {
        float dis = .5f;
        GameObject hit = raycastCreater.Raycast(5, mFort.direction, dis, true);
        if (hit != null)
        {
            DestructibleComponent destructibleComponent = hit.GetComponent<DestructibleComponent>();
           
            if((destructibleComponent!=null)&&(destructibleComponent.hurtable)) SprintHurt(hit);

            if(hit.gameObject.layer==(LayerMask.NameToLayer("harmful")))
            {
                Debug.Log("击中且击中可伤害物体");
                IfSprintDone = false;
                return false;
            }
            else
            {
                Debug.Log("击中不可穿透物体");
                IfSprintDone = true;
                return true;
            }
        }
        else
        {
            Debug.Log("未击中");
            IfSprintDone = false;
            return false;
        }
    }

    
    public void Ctor(Transform trans,MFortEntity mFort,Rigidbody2D rigid,Collider2D collider)
    {
        this.trans = transform;
        this.mFort = mFort;
        this.rigid = rigid;
        this.collider2d = collider;
        raycastCreater = new RaycastCreater(trans);
        //raycastCreater.ifShowsLog = true;
    }

    public void Run()
    {
        int direction = mFort.direction;
        float speed = 10f;
        Vector3 normalSpeed;
        if (!Hit())
        {
            normalSpeed = (new Vector3(direction, 0, 0)).normalized;
            //rigid.velocity += (Vector2)normalSpeed * speed * Time.smoothDeltaTime;
            //rigid.velocity = (Vector2)normalSpeed * speed * 40 * Time.smoothDeltaTime;
            rigid.AddForce((Vector2)normalSpeed * speed * 40 * Time.smoothDeltaTime);

        }
        else
        {
            mFort.direction *= -1;
        }
    }

    

    public void Sprint()
    {
        Hit();
        Run();

        Debug.Log("执行徘徊动作");
    }
    public void StopSprint()
    {
        rigid.velocity = Vector2.zero;
    }


    /*
    public bool IfSprintDone()
    {
        Debug.Log("执行检测是否徘徊完毕:"+ Hit());
        if(Hit())Debug.LogError("完成徘徊");
        return Hit();
    }
    */
    public bool IfSprintDone { get; set; }

}
