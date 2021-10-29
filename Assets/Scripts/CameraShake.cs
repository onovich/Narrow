using System.Collections;
using System.Collections.Generic;
using deVoid.Utils;
using UnityEngine;

  
public class CameraShake : MonoBehaviour
{
    Vector3 originalCameraPos;

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








}
