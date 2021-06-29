using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;            //X移動距離
    public float moveY = 0.0f;            //Y移動距離
    public float times = 0.0f;            //時間
    public float weight = 0.0f;            //停止時間
    public bool isMoveWhenOn = false;      //乗った時に動くフラグ
    public bool isCanMove = true;          //動くフラグ
    float perDX;                           //1フレームのX移動値
    float perDY;                           //1フレームのY移動値
    Vector3 defPos;                        //初期位置
    bool isReverse = false;                //反転フラグ

    void Start()
    {
        //初期位置
        defPos = transform.position;
        //1フレームの移動時間取得
        float timestep = Time.fixedDeltaTime;
        //1フレームのX移動値
        perDX = moveX / (1.0f / timestep * times);
        //1フレームのX移動値
        perDY = moveY / (1.0f / timestep * times);

        if (isMoveWhenOn)
        {
            //乗った時に動くので最初は動かさない
            isCanMove = false;
        }


    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isCanMove)
        {
            //移動中
            float x = transform.position.x;
            float y = transform.position.y;
            bool endX = false;
            bool endY = false;
            if (isReverse)
            {
                //逆方向移動中
                //移動量がプラスで移動位置が初期位置より小さい
                //または、移動量でマイナスで移動位置が初期位置より大きい
                if ((perDX >= 0.0f && x <= defPos.x) || (perDX < 0.0f && x >= defPos.x))
                {
                    //移動量が＋で
                    endX = true;   //X方向の移動終了
                }
                if ((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y))
                {
                    endY = true;   //Y方向の移動終了
                }
                //床を移動させる
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z));
            }
            else
            {
                //正方向移動中
                //移動量がプラスで位置が初期＋移動距離より大きい
                //または、移動量でマイナスで位置が初期＋移動距離より小さい
                if ((perDX >= 0.0f && x >= defPos.x + moveX) || (perDX < 0.0f && x <= defPos.x + moveX))
                {
                    endX = true;       //X方向の移動終了
                }
                if ((perDY >= 0.0f && y >= defPos.y + moveY) || (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    endY = true;   //Y方向の移動終了
                }
                //床を移動させる
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }
            if (endX && endY)
            {
                //移動終了
                if (isReverse)
                {
                    //正方向移動に戻る前に初期位置に戻す、そうしないと位置がずれるため
                    transform.position = defPos;
                }
                isReverse = !isReverse;     //フラグを反転させる
                isCanMove = false;          //移動フラグをおろす
                if (isMoveWhenOn == false)
                {
                    //乗った時に動くフラグOFF
                    Invoke("Move", weight);    //移動フラグを立てる遅延実行
                }
            }
        }
    }

    /// <summary>
    /// 移動フラグを立てる
    /// </summary>
    public void Move()
    {
        isCanMove = true;
    }
    
    /// <summary>
    /// 移動フラグをおろす
    /// </summary>
    public void Stop()
    {
        isCanMove = false;
    }

    /// <summary>
    /// 接触開始
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーなら移動床の子にする
            col.transform.SetParent(transform);
            //col.transform.localScale = Vector3.one;

            if (isMoveWhenOn)
            {
                //乗った時に動くフラグON
                isCanMove = true;      //移動フラグを立てる
            }
        }
    }


    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーなら移動床の子から外す
            col.transform.SetParent(null);
        }
    }

}
