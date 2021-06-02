using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] 
    public GameObject playerObj;
    [Header("コンティニュー位置")]
    public GameObject[] continuePoint;
    private PlayerController p;

    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<PlayerController>();
            if (p == null)
            {
                Debug.Log("プレイヤーじゃない物がアタッチされているよ！");
            }
        }
        else
        {
            Debug.Log("設定が足りてないよ！");
        }
    }
void Update()
    {
        if (p != null && p.IsContinueWaiting())
        {
            if (continuePoint.Length > GManager.instance.continueNum)
            {
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
                Debug.Log("スタート地点に戻る");
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りてないよ！");
            }
        }
    }
}

