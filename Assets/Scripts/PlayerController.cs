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
    private float limitPosX = 100.5f;           // 横方向の制限値
    private float limitPosY = 11.45f;          // 縦方向の制限値
    private float scale;                         // 向きの設定に利用する
    public float moveSpeed;                      // 移動速度
    public float jumpPower;                      // ジャンプ・浮遊力
    public bool isGrounded;
    bool gojump = false;
    private bool isGameOver = false;                    // GameOver状態の判定用。true ならゲームオーバー。
    public float knockbackPower;              // 敵と接触した際に吹き飛ばされる力
    public GameObject bulletPrefab;
    public Transform shotPoint;
    float coolTime = 0.3f;                       //待機時間
    float leftCoolTime;　　　　　　　　　　 // 待機している時間
    bool isRight;
    //public GManager gManager;
    private string enemyTag = "Enemy";
    [SerializeField]
    public bool isDown = false;
    private string deadAreaTag = "DeadArea";
    //public UIManager uiManager;
    private bool isJump = false;
    private bool isOtherJump = false;
    private bool isRun = false; 
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    //[Header("ジャンプする時に鳴らすSE")] public AudioClip jumpSE; 
    //[Header("やられた鳴らすSE")] public AudioClip downSE; 
    //[Header("コンティニュー時に鳴らすSE")] public AudioClip continueSE;
    

    //Rigidbody2D rb;                  //Rigidbody2D変数
    float axisH = 0.0f;              //入力
    public float speed = 3.0f;      // 移動速度  
    //public float jump = 9.0f;       //ジャンプ力
    public LayerMask groundLayer;   //着地出来るレイヤー
    bool goJump = false;            //ジャンプ開始フラグ
    bool onGround = false;          //地面に立っているフラグ

    //アニメーション対応
    Animator animator;     //アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";
    public static string gameStste = "playing";   //ゲームの状態
    public int score = 0;            // スコア
    bool isMoving = false;           //タ


    //[SerializeField, Header("Linecast用 地面判定レイヤー")]
    //private LayerMask groundLayer;
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
        scale = transform.localScale.x;

        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameStste = "playing";   //ゲーム中にする

    }
    void Update()
    {
        Shot();
        // 地面接地  Physics2D.Linecastメソッドを実行して、Ground Layerとキャラのコライダーとが接地している距離かどうかを確認し、接地しているなら true、接地していないなら false を戻す
        //isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        // Sceneビューに Physics2D.LinecastメソッドのLineを表示する
       // Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);
        if (gameStste != "playing")
        {
            return;
        }
        //移動
        if (isMoving == false)
        {
            //水平方向の入力をチェックする
            axisH = Input.GetAxisRaw("Horizontal");
        }
        //向きの調整
        if (axisH > 0.0f)
        {
            //右移動
            Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            //左移動
            Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);     //左右反転
        }
        //キャラクターをジャンプさせる
        if (Input.GetButtonDown("Jump"))
        {
           // Jump();  //ジャンプ
        }
        // ジャンプ
        //if (Input.GetButtonDown(jump) && this.rb.velocity.y == 0)
        //{
        //    Jump();
        //}

        //if (isGrounded == true)
        //{
        //    anim.ResetTrigger("Fall");

        //}
        //// 接地していない(空中にいる)間で、落下中の場合
        //if (isGrounded == false && rb.velocity.y < 0.15f)
        //{
        //    // 落下アニメを繰り返す
        //    anim.SetTrigger("Fall");
        //}
    }

    //やられた時の処理 
    private void ReceiveDamage(bool downAnim)
    {
        if (isDown)
        {
            return;
        }
        else
        {
            if (downAnim)
            {
                //anim.Play("Damage_Down");
                anim.SetTrigger("Down");
            }
            else
            {
                nonDownAnim = true;
            }
            isDown = true;
            //GManager.instance.PlaySE(downSE);  
            GManager.instance.SubLifeNum();
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
    /// ジャンプ
    /// </summary>
    public void Jump()
    {
        goJump = true;      //ジャンプフラグを立てる
        Debug.Log("ジャンプボタンを押した");
    }
    /// <summary>
    /// ジャンプと空中浮遊
    /// </summary>
    //private void Jump()
    //{


    //    // キャラの位置を上方向へ移動させる(ジャンプ・浮遊)
    //    rb.AddForce(transform.up * jumpPower);

    //    // Jump(Up + Mid) アニメーションを再生する
    //    anim.SetTrigger("Jump");
    //}

    void FixedUpdate()
    {
        //if (!isDown && !GManager.instance.isGameOver && !GManager.instance.isStageClear)
        {

        }
        //{
        //    if (gameStste != "playing")
        //    {
        //        return;
        //    }
        //    //地上判定
        //    onGround = Physics2D.Linecast(transform.position,
        //                                  transform.position - (transform.up * 0.1f),
        //                                  groundLayer);
        //    if (onGround || axisH != 0)
        //    {
        //        //地面の上or速度が０でない
        //        //速度を変更する
        //        rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
        //    }
        //    if (onGround && goJump)
        //    {
        //        //地面の上でジャンプキーが押された
        //        //ジャンプさせる
        //        Debug.Log("ジャンプ！");
        //        Vector2 jumpPw = new Vector2(0, jump);    //ジャンプさせるベクトルを作る
        //        rb.AddForce(jumpPw, ForceMode2D.Impulse);   //瞬間的な力を加える
        //        goJump = false;                           //ジャンプフラグを下ろす
        //    }
        //    if (onGround)
        //    {
        //        //地面の上
        //        if (axisH == 0)
        //        {
        //            nowAnime = stopAnime;    //停止中
        //        }
        //        else
        //        {
        //            nowAnime = moveAnime;    //移動
        //        }
        //    }
        //    else
        //    {
        //        //空中
        //        nowAnime = jumpAnime;
        //    }
        //    if (nowAnime != oldAnime)
        //    {
        //        oldAnime = nowAnime;
        //        animator.Play(nowAnime);      //アニメーション再生
        //    }
        //}


        // 移動
        //Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    //private void Move()
    //{

    //    // 水平(横)方向への入力受付
    //    float x = Input.GetAxis(horizontal);　　　// InputManager の Horizontal に登録されているキーの入力があるかどうか確認を行う

    //    // x の値が 0 ではない場合 = キー入力がある場合
    //    if (x != 0)
    //    {

    //        // velocity(速度)に新しい値を代入して移動
    //        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

    //        // temp 変数に現在の localScale 値を代入
    //        Vector3 temp = transform.localScale;

    //        // 現在のキー入力値 x を temp.x に代入
    //        temp.x = x;

    //        // 向きが変わるときに小数になるとキャラが縮んで見えてしまうので整数値にする            
    //        if (temp.x > 0)
    //        {

    //            //  数字が0よりも大きければすべて1にする
    //            temp.x = scale;

    //        }
    //        else
    //        {
    //            //  数字が0よりも小さければすべて-1にする
    //            temp.x = -scale;
    //        }

    //        // キャラの向きを移動方向に合わせる
    //        transform.localScale = temp;
    //        // 待機状態のアニメの再生を止めて、走るアニメの再生への遷移を行う
    //        anim.SetBool("Idle", false);
    //        anim.SetFloat("Run", 0.5f);
    //    }
    //    else
    //    {
    //        //  左右の入力がなかったら横移動の速度を0にしてピタッと止まるようにする
    //        rb.velocity = new Vector2(0, rb.velocity.y);
    //        //  走るアニメの再生を止めて、待機状態のアニメの再生への遷移を行う
    //        anim.SetFloat("Run", 0.0f);     // ☆　追加  Run アニメーションに対して、0.f の値を情報として渡す。遷移条件が less 0.1 なので、0.1 以下の値を渡すと条件が成立してRun アニメーションが停止される
    //        anim.SetBool("Idle", true);     // ☆　追加　Idle アニメーションを true にして、待機アニメーションを再生するS
    //    }
    //    // 現在の位置情報が移動範囲の制限範囲を超えていないか確認する。超えていたら、制限範囲内に収める
    //    float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
    //    float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

    //    // 現在の位置を更新(制限範囲を超えた場合、ここで移動の範囲を制限する)
    //    transform.position = new Vector2(posX, posY);
    //}

    /// <summary>
    /// 接触
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal")
        {
            Goal();   //ゴール
        }
        else if (col.gameObject.tag == "Dead")
        {
            //GameOver();    //ゲームオーバー
        }
        else if (col.gameObject.tag == "ScoreItem")
        {
            //スコアアイテム
            //ItenDataを取得
            ItemData item = col.gameObject.GetComponent<ItemData>();
            //スコアを取得
            score = item.value;

            //アイテム削除
            Destroy(col.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // 接触したコライダーを持つゲームオブジェクトのTagがEnemyなら 
        if (col.gameObject.tag == "Enemy")
        {
            //anim.Play("Damage_Down");
            //isDown = true;
            ReceiveDamage(true);
        }

    }
    /// <summary>
    /// ゴール
    /// </summary>
    public void Goal()
    {
        Debug.Log("ゴール");
        //animator.Play(goalAnime);
        //gameStste = "gameclear";
        //GameStop();       //ゲーム停止
    }

    /// <summary>
    /// コンティニュー待機状態か    
    /// </summary>    
    /// <returns></returns>  

    public bool IsContinueWaiting()
    {
        if (GManager.instance.isGameOver)  
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd();// || nonDownAnim;  
        }
        
    }

    private bool IsDownAnimEnd()
    {
        //Debug.Log(isDown);
        if (isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Damage_Down"))
            {
                //Debug.Log("通過");
                if (currentState.normalizedTime >= 1)
                {
                    Debug.Log("通過");
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        //animator.Play(deadAnime);
        gameStste = "gameover";
        GameStop();       //ゲーム停止
        //===================
        //ゲームオーバー演出
        //===================
        //プレイヤー当たりを消す
        GetComponent<CapsuleCollider2D>().enabled = false;
        //プレイヤーを上に少し跳ね上げる演出
        rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    /// <summary>
    /// ゲーム停止
    /// </summary>

    void GameStop()
    {
        //Rigidbody2D取得
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //速度を0にして強制停止
        rb.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// タッチスクリーン対応追加
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
    ///// <summary>
    ///// ゲームオーバー
    ///// </summary>
    //public void GameOver()
    //{
    //    isGameOver = true;

    //    // Console ビューに isGameOver 変数の値を表示する。ここが実行されると true と表示される
    //    Debug.Log(isGameOver);

    //    // 画面にゲームオーバー表示を行う
    //    //uiManager.DisplayGameOverInfo();
    //}
    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        //GManager.instance.PlaySE(continueSE);  //New! 
        isDown = false;
        anim.Play("Cindy_Idle");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        nonDownAnim = false;    
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == deadAreaTag)
    //    {
    //        ReceiveDamage(false);
    //    }
    //}
}
