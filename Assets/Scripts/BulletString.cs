using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletString : MonoBehaviour
{

    public void SetDir(int dir)
    {
        direction = dir;
    }

    [Range(-1,1)]
    public int direction = 1;

    public float speed = 5f;
    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        SetDir(direction);
    }



    // Update is called once per frame
    void Update()
    {
       
        Vector3 normalSpeed = (new Vector3(direction, 0, 0)).normalized * speed ; 
        if (normalSpeed != Vector3.zero)
        {
            rigid.velocity += (Vector2)normalSpeed * speed * Time.smoothDeltaTime;



        }
    }
}
