using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public interface IParryComponent
{
    void Ctor(PlayerEntity playerEntity,SpriteRenderer playerSprite, SpriteRenderer parrySprite,
        OnParryDoneEventHandler OnParryDoneEvent, OnParryCancelEventHandler OnParryCancelEvent,
        Sprite parryWhite, Sprite parryBlack);
    void ParryOn();
}

public delegate void OnParryDoneEventHandler();
public delegate void OnParryCancelEventHandler();

public class ParryComponent : MonoBehaviour, IParryComponent
{

    // 外部依赖
    private SpriteRenderer parrySprite;
    private SpriteRenderer playerSprite;
    PlayerEntity playerEntity;
    public Sprite parryWhite;
    public Sprite parryBlack;

    // 内部字段
    bool onParry = false;
    bool parryCold = false;
    bool reboundSuccess = false;

    // 事件
    event OnParryDoneEventHandler OnParryDoneEvent;
    event OnParryCancelEventHandler OnParryCancelEvent;

    //--------------------------------------------------------------------------

    /// 初始化+获取依赖
    public void Ctor(PlayerEntity playerEntity,SpriteRenderer playerSprite, SpriteRenderer parrySprite,
        OnParryDoneEventHandler OnParryDoneEvent, OnParryCancelEventHandler OnParryCancelEvent,
        Sprite parryWhite, Sprite parryBlack)
    {
        this.playerEntity = playerEntity;
        this.playerSprite = playerSprite;
        this.parrySprite = parrySprite;
        this.OnParryCancelEvent = OnParryCancelEvent;
        this.OnParryDoneEvent = OnParryDoneEvent;
        this.parryWhite = parryWhite;
        this.parryBlack = parryBlack;
    }

    private void Start()
    {
        parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
    }

    //--------------------------------------------------------------------------

    /// 行为层实现
    /// 对外实现
    public void ParryOn()
    {
        if ((!playerEntity.OnAttack) && (!parryCold))
        {
            StartCoroutine(ParryBeingOn());
        }
    }

    /// 内部实现
    void Rebound(BulletEntity bullet)
    {
        bullet.ChangeDir(-1);
    }
    IEnumerator ParryBeingOn()
    {
         if (!onParry)
        {
            parryCold = true;
            reboundSuccess = false;
            RaycastCreater raycast = new RaycastCreater(transform);
            GameObject collider = raycast.OverlapCirclecast("harm", 1, .5f,true);
            parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);

            //盾反落空，设置可消灭子弹，设置可被子弹攻击
            if (collider == null)
            {
                parrySprite.sprite = parryBlack;
                playerSprite.color = new Color(1, 1, 1, 1f);
                Tween tweener = parrySprite.DOColor(new Color(1, 1, 1, 1), .1f);
                parrySprite.transform.DOScale(new Vector3(1.2f, 1.2f, 1), .1f);
                onParry = true;
                OnParryCancelEvent?.Invoke();
                yield return tweener.WaitForCompletion();
                StartCoroutine(ParryBeingOff());
            }
            //盾反成功，设置不消灭子弹，设置不可被子弹攻击
            else
            {
                reboundSuccess = true;
                BulletEntity bullet = collider.gameObject.GetComponent<BulletEntity>();
                Rebound(bullet);
                bullet.ifRebound = true;
                CameraShake.instance.Shake();
                //CameraShake.instance.FlashWhite();
                collider.gameObject.GetComponent<Rigidbody2D>().velocity *= -3f;
                bullet.trail.SetActive(true);
                parrySprite.sprite = parryWhite;
                playerSprite.color = new Color(35 / 255, 35 / 255, 35 / 255, 1f);
                Tween tweener = parrySprite.DOColor(new Color(1, 1, 1, 1), .1f);
                parrySprite.transform.DOScale(new Vector3(1.2f, 1.2f, 1), .1f);
                onParry = true;
                OnParryDoneEvent?.Invoke();
                yield return tweener.WaitForCompletion();
                yield return new WaitForSeconds(.1f);
                StartCoroutine(ParryBeingOff());
            }
        }
    }
    IEnumerator ParryBeingOff()
    {
        
        if (onParry)
        {
            //盾反落空，恢复可消灭子弹，恢复可被子弹攻击
            //这里的OnParrySuccessEvent不意味着成功，而是完成
            if (!reboundSuccess)
            {
                onParry = false;
                OnParryCancelEvent?.Invoke();
                Tween tweener = parrySprite.DOColor(new Color(1, 1, 1, 0), .1f);
                parrySprite.transform.DOScale(new Vector3(1.4f, 1.4f, 1), .1f);
                yield return tweener.WaitForCompletion();
                parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
                yield return new WaitForSeconds(.2f);
                parryCold = false;
            }
            //盾反成功，恢复可消灭子弹，恢复可被子弹攻击
            else
            {
                //Debug.Log("盾反成功");
                parrySprite.sprite = parryBlack;
                parrySprite.transform.DOScale(new Vector3(1.6f, 1.6f, 1), .1f);
                Tween tweener2 = parrySprite.DOColor(new Color(1, 1, 1, 0), .1f);
                playerSprite.color = new Color(1, 1, 1, 1f);
                yield return tweener2.WaitForCompletion();
                OnParryCancelEvent?.Invoke();
                yield return new WaitForSeconds(.2f);
                parrySprite.transform.localScale = new Vector3(.5f, .5f, 1);
                parryCold = false;
                onParry = false;
            }
        }
    }
}
