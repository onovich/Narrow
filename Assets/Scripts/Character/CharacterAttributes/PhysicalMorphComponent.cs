using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IPhysicalMorphComponent
{
    void Ctor(Transform trans, TrailRenderer trail,TrailSetting setting);
    void MoveMorph();
    void StopMorph();
}

public class PhysicalMorphComponent : MonoBehaviour, IPhysicalMorphComponent
{

    Transform trans;
    TrailRenderer trail;
    float trailWidth;
    Vector3 bodySpeedUpWidth;
    float trailSpeedUpWidth;
    bool OnMorph = false;
    Tween transTweener;
    Tween trailTweener;

    public void Ctor(Transform trans, TrailRenderer trail, TrailSetting setting)
    {
        this.trans = trans;
        this.trail = trail;
        this.trailWidth = setting.trailWidth;
        this.bodySpeedUpWidth = (Vector3)setting.bodySpeedUpWidth +Vector3.forward;
        this.trailSpeedUpWidth = setting.trailSpeedUpWidth;


    }

    public void MoveMorph()
    {
        //Debug.Log("形变 ");
        if(trans!=null)trans.localScale = Vector3.Lerp(trans.localScale, bodySpeedUpWidth, .1f);
        if(trail!=null)trail.startWidth = Mathf.Lerp(trail.startWidth, trailSpeedUpWidth, .1f);
        
    }
    public void StopMorph()
    {
        
        
        if (!OnMorph)
        {
            StopAllCoroutines();
            StartCoroutine(StopingMorph());
            
        }
        
    }


    

    IEnumerator StopingMorph()
    {
        OnMorph = true;
        if (transTweener == null)
        {
            transTweener = trans.DOScale(Vector3.one, .4f);
            transTweener.SetAutoKill(false);
        }
        else
        {
            transTweener.Restart();
        }
        if (trailTweener == null)
        {
            trailTweener = trail.DOResize(trailWidth, 0, .4f);
            trailTweener.SetAutoKill(false);
        }
        else
        {
            trailTweener.Restart();
        }
        yield return transTweener.WaitForCompletion();
        OnMorph = false;

        
    }
    




}
