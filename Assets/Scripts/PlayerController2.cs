using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController2 : MonoBehaviour
{
    private bool isGameOver;
    public float knockbackPower;              // 敵と接触した際に吹き飛ばされる力
    public int coinPoint;                       // コインを獲得すると増えるポイントの総数
    //public UIManager uiManager;
    public GameObject bulletPrefab;
    public Transform shotPoint;
    float coolTime = 0.3f;                       //待機時間
    float leftCoolTime;　　　　　　　　　　 // 待機している時間
    bool isRight;
    [SerializeField]
    //private StartChecker StartChecker;

    //[SerializeField]
    private AudioClip knockbackSE;                    // 敵と接触した際に鳴らすSE用のオーディオファイルをアサインする

    [SerializeField]
    private GameObject knockbackEffectPrefab;         // 敵と接触した際に生成するエフェクト用のプレファブのゲームオブジェクトをアサインする

    [SerializeField]
    private AudioClip coinSE;                    // コインに接触した際に鳴らすSE用のオーディオファイルをアサインする

    [SerializeField]
    private GameObject coinEffectPrefab; 　　　　　　//コインと接触した際に生成するエフェクト用のプレファブのゲームオブジェクトをアサインする

    void Update()
    {
        Shot();
    }
    void Shot()
    {
        leftCoolTime -= Time.deltaTime;
        if (leftCoolTime <= 0)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, transform.rotation);

                bullet.GetComponent<BulletManeger>().Shot(transform.localScale.x / 3);
                leftCoolTime = coolTime;
            }
        }
    }
    void FixedUpdate()
    {
        if (isGameOver == true)
        {
            return;
        }
        // 移動
        //Move();

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // 接触したコライダーを持つゲームオブジェクトのTagがEnemyなら 
        if (col.gameObject.tag == "Enemy")
        {
            // キャラと敵の位置から距離と方向を計算
            Vector3 direction = (transform.position - col.transform.position).normalized;

            // 敵の反対側にキャラを吹き飛ばす
            transform.position += direction * knockbackPower;

            // 敵との接触用のSE(AudioClip)を再生する
            AudioSource.PlayClipAtPoint(knockbackSE, transform.position);

            // 接触した際のエフェクトを、敵の位置に、クローンとして生成する。生成されたゲームオブジェクトを変数へ代入
            GameObject knockbackEffect = Instantiate(knockbackEffectPrefab, col.transform.position, Quaternion.identity);

            // エフェクトを 0.5 秒後に破棄
            Destroy(knockbackEffect, 0.5f);
        }
    }




    /// <summary>
    /// バルーン破壊
    /// </summary>

    public void DestroyBallon()
    {
        // TODO 後程、バルーンが破壊される際に「割れた」ように見えるアニメ演出を追加する
        //if (ballons[1] != null)
        //{
        //    Destroy(ballons[1]);
        //}
       //else if (ballons[0] != null)
        //{
            //Destroy(ballons[0]);
        //}
    }
    ////* 新しいメソッドを１つ追加　ここから *////

    // IsTriggerがオンのコライダーを持つゲームオブジェクトを通過した場合に呼び出されるメソッド
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 通過したコライダーを持つゲームオブジェクトの Tag が Coin の場合
        if (col.gameObject.tag == "Coin")
        {
            // 通過したコインのゲームオブジェクトの持つ Coin スクリプトを取得し、point 変数の値をキャラの持つ coinPoint 変数に加算
            coinPoint += col.gameObject.GetComponent<Coin>().point;

            //uiManager.UpdateDisplayScore(coinPoint);
            // 通過したコインのゲームオブジェクトを破壊する
            Destroy(col.gameObject);

            //コインとの接触用のSE(AudioClip)を再生する
            AudioSource.PlayClipAtPoint(coinSE, transform.position);

            // 接触した際のエフェクトを、コインの位置に、クローンとして生成する。生成されたゲームオブジェクトを変数へ代入
            GameObject coinEffect = Instantiate(coinEffectPrefab, transform.position, Quaternion.identity);

            // エフェクトを 0.3 秒後に破棄
            Destroy(coinEffect, 0.3f);

        }
    }
    /// <summary>
    /// ゲームオーバー
    /// </summary>

    public void GameOver()
    {
        isGameOver = true;

        // Console ビューに isGameOver 変数の値を表示する。ここが実行されると true と表示される
        Debug.Log(isGameOver);

        // 画面にゲームオーバー表示を行う
        //uiManager.DisplayGameOverInfo();
    }
}

