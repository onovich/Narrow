using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerMovement))]
public class Dodge : MonoBehaviour
{
     RaycastCreater raycastCreater;
    Transform trans;
    Tween tweener;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        raycastCreater = new RaycastCreater(transform);
        trans = transform;
        offset = GetComponent<BoxCollider2D>().size.x;
        ghost = GetComponent<GhostAfterImageEffect>();
    }

    PlayerMovement playerMovement;
    public float leghth = 2f;
    private float offset;
    public float speed = .1f;

    GhostAfterImageEffect ghost;

    Vector3 targetTest(int direction)
    {
        GameObject obbTest = raycastCreater.Raycast(10, direction,leghth,true);
        Vector3 target;

        Vector3 diPos = direction > 0 ? Vector3.right : -Vector3.right;

        if (obbTest == null)
        {
            Debug.Log("传送：畅通无阻");
            target = transform.position + diPos * leghth;
        }
        else
        {
            Debug.Log("传送：遇到障碍");
            target = obbTest.transform.position - diPos * offset;
            //target = obbTest.transform.position;
        }
        return target;
    }
    
    IEnumerator Deliver(int direction)
    {
        //GetComponent<PlayerMovement>().trail.enabled = true;
        yield return new WaitForSecondsRealtime(.1f);
        trans.position = targetTest(direction);
         
        //yield return tweener.WaitForCompletion();
        yield return new WaitForSecondsRealtime(.1f);
        //GetComponent<PlayerMovement>().trail.enabled = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand))
        {
            int direction = playerMovement.direction;
            if (direction != 0)
            {
                StartCoroutine(Deliver(direction));
                Debug.Log("传送完成");
            }
            
        }
    }
}
