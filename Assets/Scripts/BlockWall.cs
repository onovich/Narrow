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

    public BlockState state = BlockState.blocked;

 
    public bool autoBlock = false;

    

     
     

    private void Start()
    {
        trans = transform;
        trans.localScale = state == BlockState.blocked ? new Vector3(1, targetScale, 1) : new Vector3(1, .0f, 1);
        if (autoBlock) StartCoroutine(Block());
         
    }

    public IEnumerator Block()
    {
        if(state == BlockState.unblocked)
        {
            Tween tweener = trans.DOScaleY(targetScale, blockSpeed);
            yield return tweener.WaitForCompletion();
            state = BlockState.blocked;
        }
    }
    public IEnumerator Unblock()
    {
        if (state == BlockState.blocked)
        {
            Tween tweener = trans.DOScaleY(0, unblockSpeed);
            yield return tweener.WaitForCompletion();
            state = BlockState.unblocked;
        }
    }

   
     




}
