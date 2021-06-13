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
    //[HideInInspector] 
    public bool isGameOver = false;
    //[HideInInspector] public bool isStageClear = false;
    [SerializeField]
    private ResultPopUp resultPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

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
    /// <summary> 
    /// �ŏ�����n�߂鎞�̏���    New! 
    /// </summary> 
    public void RetryGame()
    {
        isGameOver = false;
        lifeNum = defaultLifeNum;
        score = 0;
        stageNum = 1;
        continueNum = 0;
    }
    /// <summary>
    /// SE��炷
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ肳��Ă��܂���");
        }
    }
    /// <summary>
    /// ResultPopUp�̐���
    /// </summary>
    public void GenerateResultPopUp(int score)
    {
        // ResultPopUp �𐶐�
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);

        // ResultPopUp �̐ݒ���s��
        resultPopUp.SetUpResultPopUp(score);
    }
}

