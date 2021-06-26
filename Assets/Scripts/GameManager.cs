using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;     //UIを使う

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [Header("スコア")]
    public int score;
    public GameObject mainImage;       //画像を持つGameObject
    public Sprite gameOverSpr;         // GameOver画像
    public Sprite gameCleraSpr;        // GameClera画像
    public GameObject panel;           //　パネル
    public GameObject restartButton;   // RESTARTボタン
    public GameObject nextButton;      // ネクストボタン

    //+++時間制限追加+++
    public GameObject timeBar;        //時間表示イメージ
    public GameObject timeText;       //時間テキスト
    TimeController timeCnt;           //TimeController

    Image titleImage;                  // 画像を表示しているImageコンポーネント
    // +++ スコア追加 +++
    public GameObject scoreText;       　//スコアテキスト
    public static int totalScore;       //合計スコア
    public int stageScore = 0;       　//ステージスコア
    public AudioClip meGameOver;       //ゲームオーバー
    public AudioClip meGameClear;       //ゲームクリア

    //+++プレイヤー操作+++
    public GameObject inputUI;


    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        //ボタンを非表示にする
        panel.SetActive(false);
        //TimeController
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);    //制限時間なしなら隠す
            }
        }
        UpdateScore();

    }


    void Update()
    {
        if (PlayersController.gameStste == "gameclear")
        {
            //ゲームクリア
            mainImage.SetActive(true);         //画像を表示
            panel.SetActive(true);             //ボタンを表示
            //RESTARTボタンを無効にする
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameCleraSpr;     //画像を設定する
            PlayersController.gameStste = "gameend";
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;      //時間カウント停止
                //整数に代入することで少数を切り捨てる
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;    //残り時間をスコアに加える
            }
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore(); //スコア更新
            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }
            //+++プレイヤー操作+++
            inputUI.SetActive(false);     //操作UIを隠す

        }
        else if (PlayersController.gameStste == "gameover")
        {
            //ゲームオーバー
            mainImage.SetActive(true);         //画像を表示
            panel.SetActive(true);             //ボタンを表示
            //NEXTボタンを無効にする
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;     //画像を設定する
            PlayersController.gameStste = "gameend";
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;      //時間カウント停止
            }
            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
            //+++プレイヤー操作+++
            inputUI.SetActive(false);     //操作UIを隠す
        }
        else if (PlayersController.gameStste == "playing")
        {
            //ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerContyollerを取得
            PlayersController playerCnt = player.GetComponent<PlayersController>();
            //タイムを更新する
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //整数に代入することで少数を切り捨てる
                    int time = (int)timeCnt.displayTime;
                    //タイム更新
                    timeText.GetComponent<Text>().text = time.ToString();
                    //タイムオーバー
                    if (time == 0)
                    {
                        playerCnt.GameOver();       //ゲームオーバーにする
                    }
                }
                if (playerCnt.score != 0)
                {
                    stageScore += playerCnt.score;
                    playerCnt.score = 0;
                    UpdateScore();
                }
            }
        }
    }

    /// <summary>
    /// 画像を非表示にする
    /// </summary>
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    /// <summary>
    /// スコア追加
    /// </summary>
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    //+++プレイヤー操作+++
    /// <summary>
    /// ジャンプ
    /// </summary>
    public void Jump()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayersController playerCnt = player.GetComponent<PlayersController>();
        playerCnt.Jump();
    }
}