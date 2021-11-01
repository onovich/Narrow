using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryField : MonoBehaviour
{
    /*
    public Parry parry;
    private SpriteRenderer player;
    private void Start()
    {
        player = Global.instance.player.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //rebound(collision);
    }
    public bool parrySuccess = false;
    void rebound(Collider2D collision)
    {
        //Debug.Log("护盾反应");
        if (parry.onParry)
        {
            //Debug.Log("确认护盾张开");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if ((bullet != null) && (!bullet.ifRebound))
            {
                parrySuccess = true;
                player.color = new Color(1, 1, 1, 1f);
                parry.parrySprite.sprite = parry.parryBlack;

                //Debug.Log("弹反");
                parry.Rebound(bullet);
                bullet.ifRebound = true;
                CameraShake.instance.Shake();
                collision.gameObject.GetComponent<Rigidbody2D>().velocity *= -3f;
                bullet.trail.SetActive(true);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        rebound(collision);
    }
    */


}
