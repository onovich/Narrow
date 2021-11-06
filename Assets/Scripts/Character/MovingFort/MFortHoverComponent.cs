using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IFortHoverComponent
{
    void Reset();
    void Ctor();
    bool Fatigue();
}

public class MFortHoverComponent : MonoBehaviour, IFortHoverComponent
{


    public void Ctor()
    {

    }

    public bool Fatigue()
    {
        return false;
    }

    public void Reset()
    {
        
    }



}
