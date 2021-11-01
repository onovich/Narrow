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
    private SpriteRenderer player;

    Destructible destructible;
    ColliderReactor colliderReactor;

    public void Rebound(Bullet bullet)
    {
        bullet.SetDir(-bullet.direction);
    }
    ParryField parryField;
    private void Start()
    {
        //GameObject parry = TriggerCreater.instance.AddTriggerObject(.25f,transform,"parryField");
        //parryField = parry.AddComponent<ParryField>();
        //parryField.parry = this;

       

        destructible = GetComponent<Destructible>();
        colliderReactor = GetComponent<ColliderReactor>();
        parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
        player = Global.instance.player.GetComponent<SpriteRenderer>();


    }


    public Sprite parryWhite;
    public Sprite parryBlack;

    //void ParryOn()
    bool reboundSuccess = false;
    IEnumerator ParryOn()
    {
         if (!onParry)
        {
            parryCold = true;
            reboundSuccess = false;
            RaycastCreater raycast = new RaycastCreater(transform);
            //GameObject collider = raycast.Circlecast("harm", 1, .25f);
            GameObject collider = raycast.Circlecast("harm", 1, .5f);


            parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);

            //盾反落空
            if (collider == null)
            {



                parrySprite.sprite = parryBlack;
                player.color = new Color(1, 1, 1, 1f);

                Tween tweener = parrySprite.DOColor(new Color(1, 1, 1, 1), .1f);
                parrySprite.transform.DOScale(new Vector3(1.2f, 1.2f, 1), .1f);
                onParry = true;
                destructible.harmful = false;
                colliderReactor.destoryBullet = false;
                yield return tweener.WaitForCompletion();

                //yield return new WaitForSeconds(.1f);
                StartCoroutine(ParryOff());

            }
            //盾反成功
            else
            {
                reboundSuccess = true;
                Bullet bullet = collider.gameObject.GetComponent<Bullet>();
                Rebound(bullet);
                bullet.ifRebound = true;
                CameraShake.instance.Shake();
                //CameraShake.instance.FlashWhite();
                collider.gameObject.GetComponent<Rigidbody2D>().velocity *= -3f;
                bullet.trail.SetActive(true);


                parrySprite.sprite = parryWhite;
                player.color = new Color(35 / 255, 35 / 255, 35 / 255, 1f);

                Tween tweener = parrySprite.DOColor(new Color(1, 1, 1, 1), .1f);
                parrySprite.transform.DOScale(new Vector3(1.2f, 1.2f, 1), .1f);
                onParry = true;
                destructible.harmful = false;
                colliderReactor.destoryBullet = false;
                yield return tweener.WaitForCompletion();

                yield return new WaitForSeconds(.1f);
                StartCoroutine(ParryOff());

            }


           
        }
        
    }
    IEnumerator ParryOff()
    {
        
        if (onParry)
        {
            //盾反落空
            if (!reboundSuccess)
            {

                Tween tweener = parrySprite.DOColor(new Color(1, 1, 1, 0), .1f);
                parrySprite.transform.DOScale(new Vector3(1.4f, 1.4f, 1), .1f);
                yield return tweener.WaitForCompletion();
                destructible.harmful = true;
                colliderReactor.destoryBullet = true;
                parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
                parryCold = false;
                onParry = false;


            }
            //盾反成功
            else
            {
                //Debug.Log("盾反成功");
                parrySprite.sprite = parryBlack;
                Tween tweener4 = parrySprite.transform.DOScale(new Vector3(1.6f, 1.6f, 1), .1f);
                Tween tweener2 = parrySprite.DOColor(new Color(1, 1, 1, 0), .1f);
                 
              
                player.color = new Color(1, 1, 1, 1f);
                yield return tweener2.WaitForCompletion();
                yield return tweener4.WaitForCompletion();


                //parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
                //Debug.Log("盾反完成"+ parrySprite.transform.localScale);

                destructible.harmful = true;
                colliderReactor.destoryBullet = true;
                //parryField.parrySuccess = false;
                //Debug.Log("盾反检查1" + parrySprite.transform.localScale);

                yield return new WaitForSeconds(.2f);
                parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
                //Debug.Log("盾反检查2" + parrySprite.transform.localScale);

                parryCold = false;
                //Debug.Log("盾反检查3" + parrySprite.transform.localScale);

                onParry = false;
                //Debug.Log("盾反检查4" + parrySprite.transform.localScale);

            }





        }

    }

    bool parryCold = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("护盾开启");
            //ParryOn();
            if ((!destructible.OnAttack)&&(!parryCold))
            {
                StartCoroutine(ParryOn());
            }
        }
        
    }



}
