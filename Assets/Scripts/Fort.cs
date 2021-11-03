using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D),typeof(BlockWall))]
[RequireComponent(typeof(BulletManager),typeof(Destructible))]
public class Fort : MonoBehaviour
{
    public GameObject bulletPos;
    public bool autoShoot = false;
    [Range(-1,1)]
    public int direction = 1;
    BlockWall block;

    private void Start()
    {
        block = GetComponent<BlockWall>();
        GameObjectPoolManager.instance.CreatPool<BulletPool>("BulletPool");
        if (autoShoot) StartCoroutine(Shooting());

    }

    IEnumerator Shooting()
    {
        yield return new WaitUntil(() => block.state == BlockState.blocked);
        yield return new WaitForSeconds(.5f);
        while (true)
        {
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(1f);
        }
    }
    Tween tweener;
    public IEnumerator Shoot()
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



        GameObject bullet = GameObjectPoolManager.instance.GetInstance("BulletPool", bulletPos.transform.position, 10);
        bullet.GetComponent<Bullet>().SetDir(this.direction);
          
    }

}
