using System.Collections;
using System.Collections.Generic;
using deVoid.Utils;
using UnityEngine;
using DG.Tweening;


public class CameraShake : MonoBehaviour
{
    Vector3 originalCameraPos;
    Color originalColor;

    #region Singleton
    public static CameraShake instance;
    private void Awake()
    {
        instance = this;

    }
    #endregion


    private void Start()
    {
        originalCameraPos = Vector3.zero;
        originalColor = Camera.main.backgroundColor;
    }

    IEnumerator DOShake()
    {
        Vector3 offset = new Vector3(0.02f, 0.02f, 0f);
        transform.position = originalCameraPos + offset;
        yield return new WaitForSeconds(.1f);
        transform.position = originalCameraPos;
    }


    public void Shake()
    {
        StopCoroutine(DOShake());
        StartCoroutine(DOShake());
    }



    public void FlashRed()
    {
        StopCoroutine(DoFlashRed());
        StopCoroutine(DoFlashWhite());
        StartCoroutine(DoFlashRed());

    }

    public void FlashWhite()
    {
        StopCoroutine(DoFlashRed());
        StopCoroutine(DoFlashWhite());
        StartCoroutine(DoFlashWhite());

    }

    public Color red;
    public Color white;

    IEnumerator DoFlashWhite()
    {
        Tween tweener = Camera.main.DOColor(white, .1f);
        yield return tweener.WaitForCompletion();
        Camera.main.DOColor(originalColor, .1f);
    }
    IEnumerator DoFlashRed()
    {
        Tween tweener = Camera.main.DOColor(red, .1f);
        yield return tweener.WaitForCompletion();
        Camera.main.DOColor(originalColor, .1f);
    }



}
