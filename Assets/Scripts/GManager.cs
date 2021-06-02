using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [SerializeField]
    private Text txtScore;        // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������
    [Header("�X�R�A")] 
    public int score;
    [Header("���݂̃X�e�[�W")] 
    public int stageNum;
    [Header("���݂̕��A�ʒu")] 
    public int continueNum;
    [Header("���݂̎c�@")] 
    public int lifeNum;
    [Header("�f�t�H���g�̎c�@")]
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
    /// �X�R�A�\�����X�V
    /// </summary>
    /// <param name="score"></param>
    public void UpdateDisplayScore(int score)
    {
        txtScore.text = score.ToString();
    }
    /// <summary>
    /// �c�@���P���₷
    /// </summary>
    public void AddLIfeNum()
    {
        if (lifeNum < 99)
        {
            ++lifeNum;
        }
    }

    /// <summary>
    /// �c�@���P���炷
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
