using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DestructibleSetting : ScriptableObject
{
    [Range(0,10f)]
    public float maxHp;

}
