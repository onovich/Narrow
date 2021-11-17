using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TrailSetting : ScriptableObject
{
    
    public Vector2 bodySpeedUpWidth = new Vector2(1, 0.2f);
    public float trailWidth = .1f;
    public float trailSpeedUpWidth = .04f;
    public float trailLifeTime = .4f;



}
