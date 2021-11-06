using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControllerComponent))]
public class AfterImageComponent : MonoBehaviour
{
    public bool EchoOn = false;

    float timeBtwSpawns;
    public float startTimeBtwSpawns;
    public float endTime = .8f;
    public GameObject echo;
    //public GameObject[] echos;
    private PlayerControllerComponent playerMovement;
    Transform trans;
    public float startAlpha = .6f;
    public float fadeOutSpeed = .02f;
    private void Start()
    {
        playerMovement = GetComponent<PlayerControllerComponent>();
        trans = transform;
    }
 
    IEnumerator FadeOut(SpriteRenderer sprite)
    {
        float alpha = sprite.color.a;
        while (alpha > .01f)
        {
            alpha = Mathf.Lerp(alpha, 0, fadeOutSpeed);
            sprite.color = new Color(1,1,1,alpha);
            yield return null;
        }
        sprite.color = new Color(1, 1, 1, 0);
        yield return null;
        Destroy(sprite.gameObject);


    }
    private void Update()
    {
        /*
        if (EchoOn)
        {
            if (playerMovement.Horizontal != 0)
            {
                if (timeBtwSpawns <= 0)
                {
                    //int rand = Random.Range(0, echos.Length);
                    //GameObject instance = (GameObject)Instantiate(echos[rand], transform.position, Quaternion.identity);

                    GameObject instance = Instantiate(echo, trans.position, Quaternion.identity);
                    instance.transform.localScale = trans.localScale;
                    SpriteRenderer sprite = instance.GetComponent<SpriteRenderer>();
                    sprite.color = new Color(1, 1, 1, startAlpha);
                    StartCoroutine(FadeOut(sprite));
                    //Destroy(instance, endTime);
                    timeBtwSpawns = startTimeBtwSpawns;
                }
                else
                {
                    timeBtwSpawns -= Time.deltaTime;
                }
            }
        }
        */
    }


}
