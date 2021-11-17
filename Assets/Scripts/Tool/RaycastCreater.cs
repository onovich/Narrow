using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCreater
{
    public bool ifShowsLog = false;
    Transform parent;
    public RaycastCreater(Transform parent)
    {
        this.parent = parent;
    }
    

    

    /// <summary>
    /// Only for left or right in this project.
    /// </summary>
    public GameObject Raycast(int targetNum, int direction, float length, bool ignoreSelf)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = false,
        };
        return CreatRay(filter, targetNum, direction, length, ignoreSelf);
    }

    public GameObject Raycast(string layName,int targetNum,int direction,float length, bool ignoreSelf)
    {
        LayerMask layerMask = LayerMask.GetMask(layName);
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask,
        };
        return CreatRay(filter, targetNum, direction, length, ignoreSelf);
    }

    private GameObject CreatRay(ContactFilter2D filter,int targetNum, int direction, float length, bool ignoreSelf)
    {
        RaycastHit2D[] hits = new RaycastHit2D[targetNum + 2];

        Vector2 offset = direction > 0 ? parent.right : -parent.right;
        int hitNumber = Physics2D.Raycast(parent.position, offset, filter, hits, length);
        GameObject result = hitTest(hitNumber, hits, ignoreSelf);

        return result;

    }



    public GameObject OverlapCirclecast(int targetNum,  float size,bool ignoreSelf)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = false,
        };
        return CreatOverlapCircle(filter, targetNum, 0, size, ignoreSelf);
    }

    public GameObject OverlapCirclecast(string layName, int targetNum,   float size, bool ignoreSelf)
    {
        LayerMask layerMask = LayerMask.GetMask(layName);
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask,
        };
        return CreatOverlapCircle(filter, targetNum, 0, size, ignoreSelf);
    }

    public GameObject OverlapCirclecast(int targetNum,float dis, float size, bool ignoreSelf)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = false,
        };
        return CreatOverlapCircle(filter, targetNum, dis, size, ignoreSelf);
    }

    public GameObject OverlapCirclecast(string layName, float dis, int targetNum, float size, bool ignoreSelf)
    {
        LayerMask layerMask = LayerMask.GetMask(layName);
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask,
        };
        return CreatOverlapCircle(filter, targetNum, dis, size, ignoreSelf);
    }

    private GameObject CreatOverlapCircle(ContactFilter2D filter, int targetNum, float dis,  float size, bool ignoreSelf)
    {
        Collider2D[] hits = new Collider2D[targetNum + 2];
        Vector3 pos;
        if (dis == 0)
        {
            pos = parent.transform.position;
        }
        else
        {
            pos = parent.transform.position + new Vector3(dis, 0, 0);
        }
        int hitNumber = Physics2D.OverlapCircle(pos, size, filter, hits );

        GameObject result = hitTest(hitNumber,hits,ignoreSelf);

        return result;
    }

    GameObject hitTest(int hitNumber, Collider2D[] hits, bool ignoreSelf)
    {
        GameObject result = null;

        if (ignoreSelf)
        {
            if (hitNumber > 0)
            {
                if (hitNumber == 1)
                {
                    if (hits[0].gameObject == parent.gameObject)
                    {
                        if(ifShowsLog) Debug.Log("截断1:仅一个碰撞，忽略自体后无碰撞");
                        result = null;
                    }
                    else
                    {
                        if (ifShowsLog) Debug.Log("截断2:仅一个碰撞，返回碰撞");
                        result = hits[0].gameObject;
                    }
                }
                if (hitNumber > 1)
                {
                    if (hits[0].gameObject == parent.gameObject)
                    {
                        if (ifShowsLog) Debug.Log("截断3:多个碰撞，忽略自体后返回碰撞");
                        result = hits[1].gameObject;
                    }
                    else
                    {
                        if (ifShowsLog) Debug.Log("截断4:多个碰撞，返回碰撞");
                        result = hits[0].gameObject;
                    }
                }
            }
            else
            {
                if (ifShowsLog) Debug.Log("截断5:完全无碰撞");
                result = null;
            }
        }
        else
        {
            result = hitNumber > 0 ? hits[0].gameObject : null;
        }
        return result;
    }
    GameObject hitTest(int hitNumber,RaycastHit2D[] hits,bool ignoreSelf)
    {
        GameObject result = null;

        if (ignoreSelf)
        {
            if (hitNumber > 0)
            {
                if (hitNumber == 1)
                {
                    if (hits[0].collider.gameObject == parent.gameObject)
                    {
                        if (ifShowsLog) Debug.Log("截断1:仅一个碰撞，忽略自体后无碰撞");
                        result = null;
                    }
                    else
                    {
                        if (ifShowsLog) Debug.Log("截断2:仅一个碰撞，返回碰撞");
                        result = hits[0].collider.gameObject;
                    }
                }
                if (hitNumber > 1)
                {
                    if (hits[0].collider.gameObject == parent.gameObject)
                    {
                        if (ifShowsLog) Debug.Log("截断3:多个碰撞，忽略自体后返回碰撞");
                        result = hits[1].collider.gameObject;
                    }
                    else
                    {
                        if (ifShowsLog) Debug.Log("截断4:多个碰撞，返回碰撞");
                        result = hits[0].collider.gameObject;
                    }
                }
            }
            else
            {
                if (ifShowsLog) Debug.Log("截断5:完全无碰撞");
                result = null;
            }
        }
        else
        {
            result = hitNumber > 0 ? hits[0].collider.gameObject : null;
        }
        return result;
    }


}
