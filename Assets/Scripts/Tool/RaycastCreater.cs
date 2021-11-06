using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCreater
{

    Transform parent;
    public RaycastCreater(Transform parent)
    {
        this.parent = parent;
    }
    GameObject result;

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

        if (ignoreSelf)
        {
            if (hitNumber > 0)
            {
                if (hits[0] != parent.gameObject)
                {
                    result = hits[0].collider.gameObject;
                }
                else
                {
                    if (hitNumber > 1)
                    {
                        result = hits[1].collider.gameObject;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = hitNumber > 0 ? hits[0].collider.gameObject : null;

        }



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

        if (ignoreSelf)
        {
            if (hitNumber > 0)
            {
                if (hits[0] != parent.gameObject)
                {
                    result = hits[0].gameObject;
                }
                else
                {
                    if (hitNumber > 1)
                    {
                        result = hits[1].gameObject;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = hitNumber > 0 ? hits[0].gameObject : null;
        }

        

        return result;
    }



}
