using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Follow : MonoBehaviour
{
    public bool followPlayer = true;

    private void Update()
    {
        if (followPlayer)
        {
            FollowPlayer();
        }
    }

    Transform player;
    float originalZ;

    private void Start()
    {
        player = Global.instance.player.transform;
        originalZ = transform.position.z;
    }

    void FollowPlayer()
    {
        FollowTarget(player);
    }

    public void FollowTarget(Transform target)
    {
        transform.localPosition = new Vector3(target.position.x, target.position.y, originalZ);
    }

    public void MovesTo(Vector3 target,float duration)
    {
        transform.DOMove(target, duration);
    }



}
