using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class Destructible : MonoBehaviour
{
    [Range(0,10)]
    public float maxhp = 0;

    float hp = 0;

    public bool harmful = false;
    public bool destroyable = false;
    SpriteRenderer sprite;
    TrailRenderer trail;
    bool ifPlayer;
    PlayerMovement playerMovement;
    BoxCollider2D boxCollider;

    private void Start()
    {
        hp = maxhp;
        sprite = GetComponent<SpriteRenderer>();
        ifPlayer = gameObject.CompareTag("player") ? true : false;
        if (ifPlayer)
        {
            playerMovement = GetComponent<PlayerMovement>();
            trail = playerMovement.trail.GetComponent<TrailRenderer>();

        }
        boxCollider = GetComponent<BoxCollider2D>();

    }



    public ParticleSystem flash;
    public ParticleSystem explore;
 
    public bool OnAttack;
    Tween tweener;
    Tween tweener2;
    IEnumerator Flash()
    {
        OnAttack = true;
        //Debug.Log("闪烁");
        flash.Play();
        if (tweener == null)
        {
             tweener = sprite.DOColor(new Color(1, 1, 1, .5f), .2f);
            tweener.SetAutoKill(false);
            yield return tweener.WaitForCompletion();

        }
        else
        {
            tweener.Restart();
            yield return tweener.WaitForCompletion();

        }
         if (tweener2 == null)
        {
             tweener2 = sprite.DOColor(new Color(1, 1, 1, 1), .1f);
            tweener2.SetAutoKill(false);
            yield return tweener2.WaitForCompletion();

        }
        else
        {
            tweener2.Restart();
            yield return tweener2.WaitForCompletion();

        }
        OnAttack = false;

    }


    IEnumerator Explore()
    {
        
        explore.Play();
        yield return new WaitForSeconds(.2f);
        sprite.enabled = false;
        if (ifPlayer)
        {
            trail.enabled = false;
        }

        boxCollider.enabled = false;
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);

    }
    void RefreshTrail()
    {
        if (maxhp > 0)
        {
            trail.time = playerMovement.trailTime * hp / maxhp;
        }
    }

    public void GetHurt(float attackValue)
    {
        
        if (harmful)
        {
            StopCoroutine(Flash());
            StartCoroutine(Flash());
            if (ifPlayer)
            {
                Debug.Log("确实是player");
                RefreshTrail();
            }
 
            hp -= attackValue;
            if ((destroyable)&&(hp <= 0))
            {
                StartCoroutine(Explore());

            }
        }

    }



}
