using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float maxLength = 70;         //タブが動く最大距離
    public bool is4DPad = false;         //上下左右に動かすフラグ
    GameObject player;                   //操作するプレイヤーのGameObject
    Vector2 defPos;                      //タブの初期座標
    Vector2 downPos;                     //タッチ位置

    void Start()
    {
        //プレイヤーキャラを取得
        player = GameObject.FindGameObjectWithTag("Player");
        //タブの初期座標
        defPos = GetComponent<RectTransform>().localPosition;
    }

    void Update()
    {
       
    }

    /// <summary>
    /// ダウンイベント
    /// </summary>
    public void PadDown()
    {
        //マウスポイントのスクリーン座標
        downPos = Input.mousePosition;
    }

    /// <summary>
    /// ドラッグイベント
    /// </summary>
    public void PadDrag()
    {
        //マウスポイントのスクリーン座標
        Vector2 mousePosition = Input.mousePosition;
        //新しいタブの位置を求める
        Vector2 newTabPos = mousePosition - downPos;  //マウスダウン位置からの移動差分
        if (is4DPad == false)
        {
            newTabPos.y = 0;   //横スクロールの場合はY軸を0にする
        }
        //移動ベクトルを計算する
        Vector2 axis = newTabPos.normalized;    //ベクトルを正視化する
        //2点の距離を求める
        float len = Vector2.Distance(defPos, newTabPos);
        if (len > maxLength)
        {
            //限界距離を超えたので限界座標を設定する
            newTabPos.x = axis.x * maxLength;
            newTabPos.y = axis.y * maxLength;
        }
        //タブを移動させる
        GetComponent<RectTransform>().localPosition = newTabPos;
        //プレイヤーキャラを移動させる
        PlayersController plcnt = player.GetComponent<PlayersController>();
        //plcnt.SetAxis(axis.x, axis.y);
    }

    /// <summary>
    /// アップイベント
    /// </summary>
    public void PedUp()
    {
        //タブ位置の初期化
        GetComponent<RectTransform>().localPosition = defPos;
        //プレイヤーキャラを停止させる
        PlayersController plcnt = player.GetComponent<PlayersController>();
        //plcnt.SetAxis(0, 0);
    }
}
