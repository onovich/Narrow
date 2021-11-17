using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAttackComponent
{
    void Attack();
    void AttackOff();
    void Ctor(float attackValue);

}


public class AttackComponent : MonoBehaviour, IAttackComponent
{
    public float attackValue = 1f;

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void AttackOff()
    {
        throw new System.NotImplementedException();
    }

    public void Ctor(float attackValue)
    {
        this.attackValue = attackValue;
    }

    public void Ctor()
    {
        throw new System.NotImplementedException();
    }
}
