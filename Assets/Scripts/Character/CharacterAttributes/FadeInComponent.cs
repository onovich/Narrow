using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum FadeState
{
    beenIn,
    beenOut,
}
 
public interface IFadeInComponent
{
    void FadeIn();
    void FadeOut();
    void FadeOut(float targetY);
    void Ctor(Transform trans, FadeInSetting setting, FadeState state);
    void Ctor(float defaultY, Transform trans, FadeInSetting setting, FadeState state);
    bool IfFadedIn();
    bool IfFadedOut();
}

public class FadeInComponent : MonoBehaviour,IFadeInComponent
{
    // 外部依赖
    Transform trans;
    FadeInSetting setting;
    float defaultY = 1;
    float fadeInSpeed;
    float fadeOutSpeed;

    // 对外字段
    public FadeState state;

    // 内部字段 
    Tween tweener1;
    Tween tweener2;
    bool fading = false;

    /// 初始化+获取依赖
    public void Ctor(Transform trans, FadeInSetting setting,FadeState state)
    {
        this.trans = trans;
        this.setting = setting;
        this.state = state;
        this.fadeInSpeed = setting.fadeInSpeed;
        this.fadeOutSpeed = setting.fadeOutSpeed;
        trans.localScale = state == FadeState.beenIn ? new Vector3(1, 1, 1) : new Vector3(1, .0f, 1);
    }
    
    public void Ctor(float defaultY, Transform trans, FadeInSetting setting, FadeState state)
    {
        this.defaultY = defaultY;
        Ctor(trans, setting, state);
    }
    

    /// 行为层实现
    /// 对外方法
    public void FadeIn()
    {
        if ((!fading)&&(state == FadeState.beenOut))
        {
            fading = true;
            StartCoroutine(FadingIn());
        }
        
    }
    public void FadeOut(float targetY)
    {
        if ((!fading)&&(state == FadeState.beenIn))
        {
            fading = true;
            StartCoroutine(FadingOut(targetY));
        }
    }
    public void FadeOut()
    {
        if ((!fading) && (state == FadeState.beenIn))
        {
            fading = true;
            StartCoroutine(FadingOut(0));
        }
    }
    public bool IfFadedIn()
    {
        return (state == FadeState.beenIn);
    }
    public bool IfFadedOut()
    {
        return (state == FadeState.beenOut);
    }

    /// 内部实现
    public IEnumerator FadingIn()
    {
         
            if (tweener1 == null)
            {
                tweener1 = trans.DOScaleY(defaultY, fadeInSpeed);
                tweener1.SetAutoKill(false);
            }
            else
            {
                tweener1.Restart();
            }
            yield return tweener1.WaitForCompletion();
            state = FadeState.beenIn;
            fading = false;
        
    }
    public IEnumerator FadingOut(float targetY)
    {
        if (state == FadeState.beenIn)
        {
            if (tweener2 == null)
            {
                tweener2 = trans.DOScaleY(targetY, fadeOutSpeed);
                tweener2.SetAutoKill(false);
            }
            else
            {
                tweener2.Restart();
            }
            yield return tweener2.WaitForCompletion();
            state = FadeState.beenOut;
            fading = false;
        }
    }

   
}
