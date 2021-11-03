using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum BlockState
{
    unblocked,
    blocked,
}
 

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class BlockWall : MonoBehaviour
{
    Transform trans;
    public int targetScale = 1;
    public float blockSpeed = .6f;
    public float unblockSpeed = .2f;
    public ParticleSystem explode;
    public BlockState state = BlockState.unblocked;
    public bool autoBlock = false;
    Tween tweener1;
    Tween tweener2;
    private void Start()
    {
        trans = transform;
        trans.localScale = state == BlockState.blocked ? new Vector3(1, targetScale, 1) : new Vector3(1, .0f, 1);
        if (autoBlock)
        {
            state = BlockState.unblocked;
            StartCoroutine(Block());
        }
    }
    public IEnumerator Block()
    {
        if(state == BlockState.unblocked)
        {
            if (tweener1 == null)
            {
                tweener1 = trans.DOScaleY(targetScale, blockSpeed);
                tweener1.SetAutoKill(false);
            }
            else
            {
                tweener1.Restart();
            }
            yield return tweener1.WaitForCompletion();
            state = BlockState.blocked;
        }
    }
    public IEnumerator Unblock()
    {
        if (state == BlockState.blocked)
        {
            if (tweener2 == null)
            {
                tweener2 = trans.DOScaleY(0, unblockSpeed);
                tweener2.SetAutoKill(false);
            }
            else
            {
                tweener2.Restart();
            }
            yield return tweener2.WaitForCompletion();
            state = BlockState.unblocked;
        }
    }

}
