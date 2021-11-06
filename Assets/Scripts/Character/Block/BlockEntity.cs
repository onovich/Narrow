using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockEntity
{

}


public class BlockEntity : MonoBehaviour, IBlockEntity
{

    // 数据层
    public FadeInSetting fadeInSetting;
    public FadeState fadeState = FadeState.beenOut;

    // 行为层组件
    //IDestructibleComponent destructibleComponent;
    ICollideReactComponent collideReactComponent;
    IFadeInComponent fadeInComponent;

    void Ctor()
    {
        collideReactComponent = gameObject.AddComponent<CollideReactComponent>();
        collideReactComponent.Ctor(0,null,true);

        fadeInComponent = gameObject.AddComponent<FadeInComponent>();
        fadeInComponent.Ctor(transform,fadeInSetting,fadeState);
    }

    private void Awake()
    {
        Ctor();
    }
    private void Start()
    {
        fadeInComponent.FadeIn();
    }


}
