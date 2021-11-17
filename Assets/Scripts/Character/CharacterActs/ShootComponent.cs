using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IShootComponent:IAttackComponent
{
    void Ctor(Transform bulletTrans, int direction);
}

public class ShootComponent : MonoBehaviour,IShootComponent
{
    // 外部依赖
    Transform bulletTrans;

    //对外接口
    [Range(-1, 1)]
    public int direction;

    //内部字段
    Tween tweener;
    bool onAttack = false;

    ///初始化
    ///获取依赖
    ///创建对象池
    public void Ctor(Transform bulletTrans,int direction)
    {
        this.bulletTrans = bulletTrans;
        this.direction = direction;
    }

    public void Ctor(float attackValue)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        GameObjectPoolManager.instance.CreatPool<BulletPool>("BulletPool");
    }

    //--------------------------------------------------------------------------

    /// 行为层实现
    /// 对外方法
    
    public void Attack()
    {
        if (!onAttack)
        {
            StartCoroutine(Shooting());
            onAttack = true;
        }
    }
    
    public void AttackOff()
    {
        StopAllCoroutines();
        //StopCoroutine(Shooting());
        onAttack = false;
    }
    /// 内部实现
    /// 主循环+循环间隔
    IEnumerator Shooting()
    {
        while (true)
        {
            StartCoroutine(ShootOnce());
            yield return new WaitForSeconds(1f);
        }
    }
    /// 循环对象
    IEnumerator ShootOnce()
    {
        if (tweener == null)
        {
            tweener = transform.DOPunchScale(Vector3.one * .1f, .2f);
            tweener.SetAutoKill(false);
        }
        else
        {
            tweener.Restart();
        }
        yield return tweener.WaitForCompletion();
        GameObject bullet = GameObjectPoolManager.instance.GetInstance("BulletPool", bulletTrans.position, 10);
        bullet.GetComponent<BulletEntity>().Ctor(this.direction);
    }

    
}
