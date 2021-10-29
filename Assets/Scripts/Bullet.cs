using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public void SetDir(int dir)
    {
        direction = dir;
    }

    //public ParticleSystem explode;

    [Range(-1,1)]
    public int direction = 1;

    public float speed = 5f;
    Rigidbody2D rigid;

    private LayerMask player;
    private LayerMask wall;
    ContactFilter2D playerFilter;
    ContactFilter2D wallFilter;

    Transform trans;
    SpriteRenderer sprite;


    void Start()
    {
        trans = transform;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        SetDir(direction);

        player = LayerMask.GetMask("player");
        wall = LayerMask.GetMask("OBB");

        playerFilter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = player,
        };
        wallFilter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = wall,
        };

    }


    // Update is called once per frame
    void Update()
    {
        Vector3 normalSpeed = (new Vector3(direction, 0, 0)).normalized * speed ; 
        if (normalSpeed != Vector3.zero)
        {
            rigid.velocity += (Vector2)normalSpeed * speed * Time.smoothDeltaTime;



        }

        RaycastHit2D[] playerHits = new RaycastHit2D[36];
        RaycastHit2D[] wallHits = new RaycastHit2D[36];

        float playerDis = .1f;
        float wallDis = .1f;

        Vector2 bulletOffset = direction > 0 ? trans.right : -trans.right;
        int playerHitNumber = Physics2D.Raycast(trans.position, bulletOffset * playerDis, playerFilter, playerHits, playerDis);
        int wallHitNumber = Physics2D.Raycast(trans.position, bulletOffset * wallDis, wallFilter, wallHits, wallDis);


        
        if (playerHitNumber > 0)
        {
            
            //gameObject.SetActive(false);

        }
        if (wallHitNumber > 0)
        {
            
            BlockWall blockWall = wallHits[0].collider.gameObject.GetComponent<BlockWall>();
            if (blockWall)
            {
            }

            //gameObject.SetActive(false);

        }
        

    }
}
