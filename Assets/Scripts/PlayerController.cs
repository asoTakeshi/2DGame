using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";    // キー入力用の文字列指定(InputManager の Horizontal の入力を判定するための文字列)
    private string jump = "Jump";　　　　　　　　// キー入力用の文字列指定
    private Rigidbody2D rb;                      // コンポーネントの取得用
    private Animator anim;
    private float limitPosX = 45.5f;           // 横方向の制限値
    private float limitPosY = 11.45f;          // 縦方向の制限値
    private float scale;                         // 向きの設定に利用する
    public float moveSpeed;                      // 移動速度
    public float jumpPower;                      // ジャンプ・浮遊力
    public bool isGrounded;
    bool gojump = false;
    private bool isGameOver;                     // GameOver状態の判定用。true ならゲームオーバー。
    public float knockbackPower;              // 敵と接触した際に吹き飛ばされる力
    public int gemPoint;                       // コインを獲得すると増えるポイントの総数
    public GameObject bulletPrefab;
    public Transform shotPoint;
    float coolTime = 0.3f;                       //待機時間
    float leftCoolTime;　　　　　　　　　　 // 待機している時間
    bool isRight;
    public int coinPoint;
    public GManager gManager;



    [SerializeField, Header("Linecast用 地面判定レイヤー")]
    private LayerMask groundLayer;
    //[SerializeField]
    // private AudioClip knockbackSE;                    // 敵と接触した際に鳴らすSE用のオーディオファイルをアサインする

    [SerializeField]
    private GameObject knockbackEffectPrefab;         // 敵と接触した際に生成するエフェクト用のプレファブのゲームオブジェクトをアサインする

    //[SerializeField]
    //private AudioClip coinSE;                    // コインに接触した際に鳴らすSE用のオーディオファイルをアサインする

    [SerializeField]
    private GameObject coinEffectPrefab; 　　　　　　//コインと接触した際に生成するエフェクト用のプレファブのゲームオブジェクトをアサインする

    void Start()
    {
        // 必要なコンポーネントを取得して用意した変数に代入
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        scale = transform.localScale.x;
    }
    void Update()
    {
        Shot();
        // 地面接地  Physics2D.Linecastメソッドを実行して、Ground Layerとキャラのコライダーとが接地している距離かどうかを確認し、接地しているなら true、接地していないなら false を戻す
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        // Sceneビューに Physics2D.LinecastメソッドのLineを表示する
        Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);
        //if (ballons.Length > 0)
        //{


        // ジャンプ
        if (Input.GetButtonDown(jump) && this.rb.velocity.y == 0)
        {
            Jump();
        }

        if (isGrounded == true)
        {
            anim.ResetTrigger("Fall");

        }
        // 接地していない(空中にいる)間で、落下中の場合
        if (isGrounded == false && rb.velocity.y < 0.15f)
        {

            //if(anim.GetCurrentAnimatorStateInfo(0).IsName ("Jump_Up"))
            //{

            //}

            // 落下アニメを繰り返す
            anim.SetTrigger("Fall");
        }

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

    /// <summary>
    /// ジャンプと空中浮遊
    /// </summary>
    private void Jump()
    {


        // キャラの位置を上方向へ移動させる(ジャンプ・浮遊)
        rb.AddForce(transform.up * jumpPower);

        // Jump(Up + Mid) アニメーションを再生する
        anim.SetTrigger("Jump");
    }

    void FixedUpdate()
    {
        
        // 移動
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {

        // 水平(横)方向への入力受付
        float x = Input.GetAxis(horizontal);　　　// InputManager の Horizontal に登録されているキーの入力があるかどうか確認を行う

        // x の値が 0 ではない場合 = キー入力がある場合
        if (x != 0)
        {

            // velocity(速度)に新しい値を代入して移動
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

            // temp 変数に現在の localScale 値を代入
            Vector3 temp = transform.localScale;

            // 現在のキー入力値 x を temp.x に代入
            temp.x = x;

            // 向きが変わるときに小数になるとキャラが縮んで見えてしまうので整数値にする            
            if (temp.x > 0)
            {

                //  数字が0よりも大きければすべて1にする
                temp.x = scale;

            }
            else
            {
                //  数字が0よりも小さければすべて-1にする
                temp.x = -scale;
            }

            // キャラの向きを移動方向に合わせる
            transform.localScale = temp;
            // 待機状態のアニメの再生を止めて、走るアニメの再生への遷移を行う
            anim.SetBool("Idle", false);
            anim.SetFloat("Run", 0.5f);
        }
        else
        {
            //  左右の入力がなかったら横移動の速度を0にしてピタッと止まるようにする
            rb.velocity = new Vector2(0, rb.velocity.y);
            //  走るアニメの再生を止めて、待機状態のアニメの再生への遷移を行う
            anim.SetFloat("Run", 0.0f);     // ☆　追加  Run アニメーションに対して、0.f の値を情報として渡す。遷移条件が less 0.1 なので、0.1 以下の値を渡すと条件が成立してRun アニメーションが停止される
            anim.SetBool("Idle", true);     // ☆　追加　Idle アニメーションを true にして、待機アニメーションを再生するS
        }
        // 現在の位置情報が移動範囲の制限範囲を超えていないか確認する。超えていたら、制限範囲内に収める
        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        // 現在の位置を更新(制限範囲を超えた場合、ここで移動の範囲を制限する)
        transform.position = new Vector2(posX, posY);
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
            //AudioSource.PlayClipAtPoint(knockbackSE, transform.position);

            // 接触した際のエフェクトを、敵の位置に、クローンとして生成する。生成されたゲームオブジェクトを変数へ代入
            GameObject knockbackEffect = Instantiate(knockbackEffectPrefab, col.transform.position, Quaternion.identity);

            // エフェクトを 0.5 秒後に破棄
            Destroy(knockbackEffect, 0.5f);
        }
    }
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    // 通過したコライダーを持つゲームオブジェクトの Tag が Coin の場合
    //    if (col.gameObject.tag == "gem")
    //    {
    //        // 通過したコインのゲームオブジェクトの持つ Coin スクリプトを取得し、point 変数の値をキャラの持つ coinPoint 変数に加算
    //        gemPoint += col.gameObject.GetComponent<Coin>().point;

    //        gManager.UpdateDisplayScore(coinPoint);
    //        // 通過したコインのゲームオブジェクトを破壊する
    //        Destroy(col.gameObject);

    //        //コインとの接触用のSE(AudioClip)を再生する
    //        //AudioSource.PlayClipAtPoint(coinSE, transform.position);

    //        // 接触した際のエフェクトを、コインの位置に、クローンとして生成する。生成されたゲームオブジェクトを変数へ代入
    //        GameObject coinEffect = Instantiate(coinEffectPrefab, transform.position, Quaternion.identity);

    //        // エフェクトを 0.3 秒後に破棄
    //        Destroy(coinEffect, 0.3f);

    //    }
    //}
    
}
