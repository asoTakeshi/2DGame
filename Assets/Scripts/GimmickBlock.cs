using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;      //自動落下検知距離
    public bool isDelete = false;    //落下後に削除するフラグ

    bool isFell = false;            //落下フラグ
    float fadeTime = 0.5f;          //フェードアウト時間
    
    void Start()
    {
        //Rigidbody2Dの挙動を止める
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
    }

    
    void Update()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");  //プレイヤーを探す
        if (player != null)
        {
            //プレイヤーとの距離を計測
            float d = Vector2.Distance(
                transform.position,player.transform.position);
            if(length >= d)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if(rb.bodyType == RigidbodyType2D.Static)
                {
                    // Rigidbody2Dの物理挙動を開始
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        if (isFell)
        {
            //落下した
            //透明度を変更してフェードアウトされる
            fadeTime -= Time.deltaTime;                      //前フレームの差分秒マイナス
            Color col = GetComponent<SpriteRenderer>().color;    //カラーを取り出す
            col.a = fadeTime;     //透明度を変更
            GetComponent<SpriteRenderer>().color = col;      //カラーを再設定
            if (fadeTime <= 0.0f)
            {
                //0以下(透明)になったら消す
                Destroy(gameObject);
            }


        }
    }
    /// <summary>
    /// 接触開始
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            isFell = true;     //落下フラグオン
        }
    }
}
