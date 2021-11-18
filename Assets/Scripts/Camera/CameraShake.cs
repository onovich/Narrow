using System.Collections;
using System.Collections.Generic;
using deVoid.Utils;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;//使用Volume所需



public class CameraShake : MonoBehaviour
{
    Vector3 originalCameraPos;
    Color originalColor;
    private Volume volume;
    private ColorCurves colorCurve;


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

        volume = Camera.main.GetComponent<Volume>();
        ColorCurves tmp1;
        if (volume.profile.TryGet<ColorCurves>(out tmp1))
        {
            colorCurve = tmp1;
        }
        colorCurve.active = false;

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

    public void FlashBlackAndWhite()
    {
        StopCoroutine(DoFlashBlackAndWhite());
        StartCoroutine(DoFlashBlackAndWhite());
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
    
    IEnumerator DoFlashBlackAndWhite()
    {
        colorCurve.active = true;
        yield return new WaitForSeconds(.2f);
        colorCurve.active = false;
    }
    

}
