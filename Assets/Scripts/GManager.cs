using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;        // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������
    public static GManager instance = null;
    public int score;
    public int stageNum;
    public int continueNum;
    public int lifeNum;

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
}
