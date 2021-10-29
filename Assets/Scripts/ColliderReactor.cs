using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class ColliderReactor : MonoBehaviour
{

    public ParticleSystem explode;

    [Range(0,1)]
    public int direction = 1;

    [HideInInspector]
    public bool destoryBullet = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (destoryBullet)
        {
            if (collision.gameObject.CompareTag("bullet"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        
        
        

        if (explode!=null)
        {
            explode.Play();
        }
        CameraShake.instance.Shake();
    }





}
