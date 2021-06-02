using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [SerializeField]
    private Text txtScore;        // txtScore ゲームオブジェクトの持つ Text コンポーネントをインスペクターからアサインする
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
    [HideInInspector] 
    public bool isGameOver = false;

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
    /// <summary>
    /// スコア表示を更新
    /// </summary>
    /// <param name="score"></param>
    public void UpdateDisplayScore(int score)
    {
        txtScore.text = score.ToString();
    }
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
}
