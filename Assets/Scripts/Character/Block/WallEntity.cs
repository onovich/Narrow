using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEntity : MonoBehaviour
{
    // 数据层
    [Range(-1, 1)]
    public int direction;
    public ParticleSystem collideEffect;

    // 行为层组件
    //IDestructibleComponent destructibleComponent;
    ICollideReactComponent collideReactComponent;
    IFadeInComponent fadeInComponent;
    public FadeInSetting fadeInSetting;
    public FadeState fadeState = FadeState.beenOut;

    void Ctor()
    {
        collideReactComponent = gameObject.AddComponent<CollideReactComponent>();
        collideReactComponent.Ctor(direction, collideEffect,true);

        fadeInComponent = gameObject.AddComponent<FadeInComponent>();
        fadeInComponent.Ctor(10f,transform, fadeInSetting, fadeState);
    }

    private void Awake()
    {
        Ctor();
        float offsetY = direction;
        collideEffect.transform.localScale = new Vector3(collideEffect.transform.localScale.x, collideEffect.transform.localScale.y * offsetY, collideEffect.transform.localScale.z); 
    }
    private void Start()
    {
        fadeInComponent.FadeIn();
    }
}
