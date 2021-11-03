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



    public GameObject Circlecast(int targetNum,  float length,bool ignoreSelf)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = false,
        };
        return CreatCircle(filter, targetNum, length, ignoreSelf);
    }

    public GameObject Circlecast(string layName, int targetNum,   float length, bool ignoreSelf)
    {
        LayerMask layerMask = LayerMask.GetMask(layName);
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask,
        };
        return CreatCircle(filter, targetNum,  length, ignoreSelf);
    }



    private GameObject CreatCircle(ContactFilter2D filter, int targetNum,   float length, bool ignoreSelf)
    {
        Collider2D[] hits = new Collider2D[targetNum + 2];

        int hitNumber = Physics2D.OverlapCircle(parent.position, length, filter, hits );

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
