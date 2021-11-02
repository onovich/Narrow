using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeController : MonoBehaviour
{



    void Start()
    {
        
    }

     void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("时间变慢");
            Time.timeScale = Mathf.Lerp(Time.timeScale, .4f, .05f);
            //Time.timeScale = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1;
        }
    }
}
