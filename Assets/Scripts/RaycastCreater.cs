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
    public GameObject Raycast(int targetNum, int direction, float length)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = false,
        };
        return CreatRay(filter, targetNum, direction, length);
    }

    public GameObject Raycast(string layName,int targetNum,int direction,float length)
    {
        LayerMask layerMask = LayerMask.GetMask(layName);
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask,
        };
        return CreatRay(filter, targetNum, direction, length);
    }

    private GameObject CreatRay(ContactFilter2D filter,int targetNum, int direction, float length)
    {
        RaycastHit2D[] hits = new RaycastHit2D[targetNum + 1];

        Vector2 offset = direction > 0 ? parent.right : -parent.right;
        int hitNumber = Physics2D.Raycast(parent.position, offset, filter, hits, length);

        result = hitNumber > 0 ? hits[0].collider.gameObject : null;
        return result;
    }



    public GameObject Circlecast(int targetNum,  float length)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = false,
        };
        return CreatCircle(filter, targetNum, length);
    }

    public GameObject Circlecast(string layName, int targetNum,   float length)
    {
        LayerMask layerMask = LayerMask.GetMask(layName);
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask,
        };
        return CreatCircle(filter, targetNum,  length);
    }



    private GameObject CreatCircle(ContactFilter2D filter, int targetNum,   float length)
    {
        Collider2D[] hits = new Collider2D[targetNum + 1];

        int hitNumber = Physics2D.OverlapCircle(parent.position, length, filter, hits );

        result = hitNumber > 0 ? hits[0].gameObject : null;
        return result;
    }



}
