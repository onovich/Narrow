using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryField : MonoBehaviour
{
    public Parry parry;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("护盾反应");
        if (parry.onParry)
        {
            Debug.Log("确认护盾张开");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Debug.Log("弹反");
                parry.Rebound(bullet);
            }
        }
    }
     



}
