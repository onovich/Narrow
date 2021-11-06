using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class PlayerSetting : ScriptableObject
{
    public float moveSpeed = 10f;
    public Vector2 speedUpScale = new Vector2(1, 0.2f);
    public float trailWidth = .1f;
    public float trailWidthScale = .04f;

    public float maxHP;
}
