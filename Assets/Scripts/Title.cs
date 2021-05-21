using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private bool firstPush = false;
    // スタートボタンが押されると呼ばれる
    public void PressStart()
    {

        if (!firstPush)
        {
            
            //次のシーンに行く命令を書く

            firstPush = true;
        }
    }
  
}
