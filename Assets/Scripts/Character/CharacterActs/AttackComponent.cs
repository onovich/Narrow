using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAttackComponent
{
    void Attack();
    void AttackOff();

}


public class AttackComponent : MonoBehaviour
{
    public float attackValue = 1f;


}
