using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Destructible : MonoBehaviour
{
    [Range(0,10)]
    public float maxhp = 0;

    float hp = 0;

    public bool harmful = false;
    public bool destroyable = false;
    SpriteRenderer sprite;
    private void Start()
    {
        hp = maxhp;
        sprite = GetComponent<SpriteRenderer>();
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Attack attack = collision.gameObject.GetComponent<Attack>();
        //存在攻击对象时
        if (attack!=null)
        {
            GetHurt(attack.attackValue);
        }
        
    }
    */

    public ParticleSystem flash;

    /*
    void Flash()
    {
        flash.Play();
    }
    */
    public bool OnAttack;
    IEnumerator Flash()
    {
        OnAttack = true;
        //Debug.Log("闪烁");
        //flash.Play();
        Tween tweener = sprite.DOColor(new Color(1,1,1,.5f),.2f);
        //Tween tweener = sprite.DOBlendableColor(new Color(255, 255, 255, 0), 1f);
        yield return tweener.WaitForCompletion();
        Tween tweener2 = sprite.DOColor(new Color(1,1,1,1), .1f);
        yield return tweener2.WaitForCompletion();
        OnAttack = false;

    }


    void Destroy()
    {

    }

    public void GetHurt(float attackValue)
    {
        
        if (harmful)
        {
            StopCoroutine(Flash());
            StartCoroutine(Flash());
            //Flash();

            hp -= attackValue;
            if ((destroyable)&&(hp <= 0))
            {
                Destroy();
            }
        }

    }



}
