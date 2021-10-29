using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Silent.GameSystem.SearchObject;




public class Global : MonoBehaviour
{ 
    public GameObject player;
    [HideInInspector]
    public SearchGameObject sgo;

    #region Singleton
    public static Global instance;
     private void Awake()
    {
        instance = this;
        sgo = gameObject.AddComponent<SearchGameObject>();

    }
    #endregion

    private void Start()
    {
        
    }


}
