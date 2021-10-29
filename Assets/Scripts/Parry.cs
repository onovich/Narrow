using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Destructible),typeof(ColliderReactor))]
public class Parry : MonoBehaviour
{

    public SpriteRenderer parrySprite;
    [HideInInspector]
    public bool onParry = false;

    Destructible destructible;
    ColliderReactor colliderReactor;

    public void Rebound(Bullet bullet)
    {
        //particle.Play();
        bullet.SetDir(-bullet.direction);

    }

    private void Start()
    {
        GameObject parry = TriggerCreater.instance.AddTriggerObject(.25f,transform,"parry");
        ParryField parryField = parry.AddComponent<ParryField>();
        parryField.parry = this;
        destructible = GetComponent<Destructible>();
        colliderReactor = GetComponent<ColliderReactor>();

    }

    //void ParryOn()
    IEnumerator ParryOn()
    {
         if (!onParry)
        {
            Tween tweener = parrySprite.DOColor(new Color(1,1,1,1), .1f);
            onParry = true;
            destructible.harmful = false;
            colliderReactor.destoryBullet = false;
            yield return tweener.WaitForCompletion();
            yield return new WaitForSeconds(.2f);
            


            Debug.Log("护盾关闭");
            ParryOff();
        }
        

        

    }
    void ParryOff()
    {
        
        if (onParry)
        {
            parrySprite.DOColor(new Color(1,1,1,0), .1f);
            onParry = false;
            destructible.harmful = true;
            colliderReactor.destoryBullet = true;

        }
        
 
        

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("护盾开启");
            //ParryOn();
            StartCoroutine(ParryOn());
        }
        /*
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("护盾关闭");
            ParryOff();
        }
        */
    }



}
