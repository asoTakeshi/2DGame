using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [Header("スコア")]
    public int score;
    [Header("現在のステージ")]
    public int stageNum;
    [Header("現在の復帰位置")]
    public int continueNum;
    [Header("現在の残機")]
    public int lifeNum;
    [Header("デフォルトの残機")]
    public int defaultLifeNum;
    //[HideInInspector] 
    public bool isGameOver = false;
    //[HideInInspector] public bool isStageClear = false;
    [SerializeField]
    private ResultPopUp resultPopUpPrefab;
    [HideInInspector] 
    public bool isStageClear = false;

    private AudioSource audioSource = null;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// スコア表示を更新
    /// </summary>
    /// <param name="score"></param>
    //public void UpdateDisplayScore(int score)
    //{
    //    txtScore.text = score.ToString();
    //}
    /// <summary>
    /// 残機を１つ増やす
    /// </summary>
    public void AddLIfeNum()
    {
        if (lifeNum < 99)
        {
            ++lifeNum;
        }
    }

    /// <summary>
    /// 残機を１つ減らす
    /// </summary>
    public void SubLifeNum()
    {
        if (lifeNum > 0)
        {
            --lifeNum;
        }
        else
        {
            isGameOver = true;
        }
    }
    /// <summary> 
    /// 最初から始める時の処理    
    /// </summary> 
    public void RetryGame()
    {
        isGameOver = false;
        lifeNum = defaultLifeNum;
        score = 0;
        //stageNum = 1;
        continueNum = 0;
    }
    /// <summary>
    /// SEを鳴らす
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }
    /// <summary>
    /// ResultPopUpの生成
    /// </summary>
    public void GenerateResultPopUp()
    {
        //Debug.Log(score);
        Transform canvasTran = GameObject.FindGameObjectWithTag("Canvas").transform;
        // ResultPopUp を生成
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);

        // ResultPopUp の設定を行う
        resultPopUp.SetUpResultPopUp(score);
    }
}

