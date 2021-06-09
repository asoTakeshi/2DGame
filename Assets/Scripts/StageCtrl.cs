using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("�v���C���[�Q�[���I�u�W�F�N�g")] 
    public GameObject playerObj;
    [Header("�R���e�B�j���[�ʒu")]
    public GameObject[] continuePoint;
    [Header("�Q�[���I�[�o�[")] public GameObject gameOverObj;
    [Header("�t�F�[�h")] public FadeImage fade;
    //[Header("�Q�[���I�[�o�[���ɖ炷SE")] public AudioClip gameOverSE; 
    //[Header("���g���C���ɖ炷SE")] public AudioClip retrySE; 
    //[Header("�X�e�[�W�N���A�[SE")] public AudioClip stageClearSE;
    [Header("�X�e�[�W�N���A")] public GameObject stageClearObj;
    [Header("�X�e�[�W�N���A����")] public PlayerTriggerCheck stageClearTrigger;

    private int nextStageNum; 
    private bool startFade = false; 
    private bool doGameOver = false; 
    private bool retryGame = false; 
    private bool doSceneChange = false; 
    private PlayerController p;
    private bool doClear = false;

    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0 && gameOverObj != null && fade != null && stageClearObj != null)
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<PlayerController>();
            if (p == null)
            {
                Debug.Log("�v���C���[����Ȃ������A�^�b�`����Ă����I");
            }
        }
        else
        {
            Debug.Log("�ݒ肪����ĂȂ���I");
        }
    }
    void Update()
    {
        //�Q�[���I�[�o�[���̏���
        if (GManager.instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            //GManager.instance.PlaySE(gameOverSE); 
            doGameOver = true;
        }
        //�v���C���[�����ꂽ���̏���
        else if (p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if (continuePoint.Length > GManager.instance.continueNum)
            {
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("�R���e�B�j���[�|�C���g�̐ݒ肪����ĂȂ���I");
            }
        }
        else if (stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;
        }
    
        //�X�e�[�W��؂�ւ���
        if (fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeOutComplete())
            {
                //�Q�[�����g���C
                if (retryGame)
                {
                    GManager.instance.RetryGame();
                }
                //���̃X�e�[�W
                else
                {
                    GManager.instance.stageNum = nextStageNum;
                }
                GManager.instance.isStageClear = false;
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
            }
        }
    }

    /// <summary>
    /// �ŏ�����n�߂� New!
    /// </summary>
    public void Retry()
    {
        ChangeScene(1); //�ŏ��̃X�e�[�W�ɖ߂�̂łP
        //GManager.instance.PlaySE(retrySE); 
        retryGame = true;
    }

    /// <summary>
    /// �X�e�[�W��؂�ւ��܂��B New!
    /// </summary>
    /// <param name="num">�X�e�[�W�ԍ�</param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }
    /// <summary>
    /// �X�e�[�W���N���A����
    /// </summary>
    public void StageClear()
    {
        GManager.instance.isStageClear = true;
        stageClearObj.SetActive(true);
        //GManager.instance.PlaySE(stageClearSE);
    }
}


