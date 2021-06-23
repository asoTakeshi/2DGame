using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f;      //íœ‚·‚éŠÔw’è

    
    void Start()
    {
        Destroy(gameObject, deleteTime);    //íœİ’è
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);     //‰½‚©‚ÉÚG‚µ‚½‚çÁ‚·
    }
}
