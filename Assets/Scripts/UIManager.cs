using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;   // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;

    [SerializeField]
    //private ResultPopUp resultPopUpPrefab;

    //[SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private Button btnInfo;

    [SerializeField]
    private Button btnTitle;

    [SerializeField]
    private Text lblStart;

    [SerializeField]
    private CanvasGroup canvasGroupTitle;

    Tweener tweener;



    /// <summary>
    /// �X�R�A�\�����X�V
    /// </summary>
    /// <param name="score"></param>

    public void UpdateDisplayScore(int score)
    {
        txtScore.text = score.ToString();

    }

    ////* �V�������\�b�h���P�ǉ��B�������� *////

    /// <summary>
    /// �Q�[���I�[�o�[�\��
    /// </summary>

    public void DisplayGameOverInfo()
    {
        // InfoBackGround �Q�[���I�u�W�F�N�g�̎��� CanvasGroup �R���|�[�l���g�� Alpha �̒l���A1�b������ 1 �ɕύX���āA�w�i�ƕ�������ʂɌ�����悤�ɂ���
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        // ��������A�j���[�V���������ĕ\��
        txtInfo.DOText("Game Over...", 1.0f);

        btnInfo.onClick.AddListener(RestartGame);
    }
    /// <summary>
    /// ResultPopUp�̐���
    /// </summary>

    public void GenerateResultPopUp(int score)
    {
        // ResultPopUp �𐶐�
        //ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);

        // ResultPopUp �̐ݒ���s��
        //resultPopUp.SetUpResultPopUp(score);
    }
    /// <summary>
    /// �^�C�g���֖߂�
    /// </summary>
    public void RestartGame()
    {
        // �{�^�����烁�\�b�h���폜(�d���N���b�N�h�~)
        btnInfo.onClick.RemoveAllListeners();

        // ���݂̃V�[���̖��O���擾
        string sceneName = SceneManager.GetActiveScene().name;

        canvasGroupInfo.DOFade(0f, 1.0f)
            .OnComplete(() =>
            {
                Debug.Log("Restart");
                SceneManager.LoadScene(sceneName);

            });

    }
    private void Start()
    {
        // �^�C�g���\��
        SwitchDisplayTitle(true, 1.0f);

        // �{�^����OnClick�C�x���g�Ƀ��\�b�h��o�^
        //btnTitle.onClick.AddListener(OnClickTitle);
    }
    /// <summary>
    /// �^�C�g���\���i�������R�����g�ŏ����Ă݂܂��傤�j
    /// </summary>

    //�^�C�g���ĂԊ֐� 
    public void SwitchDisplayTitle(bool isSwitch, float alpha)
    {
        // �^�C�g���̏ꍇ�ɂ́AcanvasGroupTitle�̓����x��0�ɂ���B
        if (isSwitch) canvasGroupTitle.alpha = 0;

        //canvasGroupTitle �� Alpha �̒l���A1�b�����āAalpha �ϐ��̒l(����� 1.0f)�ɁA���{���ŕύX���� 
        canvasGroupTitle.DOFade(alpha, 1.0f).SetEase(Ease.Linear).OnComplete(() => {

            // isSwitch = true �ł��邽�߁A�����Ƃ��Ă͈ȉ��̂悤�ɂȂ��Ă���B
            // ���̂��߁ASetActive ���\�b�h�� true �Ŏ��s����Ă���̂ŁAlblStart.gameObject��\�����鏈���ɂȂ�
            lblStart.gameObject.SetActive(isSwitch);
        });
        if (tweener == null)
        {
            // Tap Start�̕������������_�ł�����
            tweener = lblStart.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            tweener.Kill();
        }
    }
    /// <summary>
    /// �^�C�g���\�����ɉ�ʂ��N���b�N�����ۂ̏���
    /// </summary>
    private void OnClickTitle()
    {
        // �{�^���̃��\�b�h���폜���ďd���^�b�v�h�~
        btnTitle.onClick.RemoveAllListeners();

        // �^�C�g�������X�ɔ�\��
        SwitchDisplayTitle(false, 0.0f);

        // �^�C�g���\����������̂Ɠ���ւ��ŁA�Q�[���X�^�[�g�̕�����\������
        StartCoroutine(DisplayGameStartInfo());
    }
    /// <summary>
    /// �Q�[���X�^�[�g�\���i�������R�����g�ŏ����Ă݂܂��傤�j
    /// </summary>
    /// <returns></returns>

    //�R���[�`����錾����
    public IEnumerator DisplayGameStartInfo()
    {
        //0.5�b�҂�
        yield return new WaitForSeconds(0.5f);

        //canvasGroupInfo�̓����x��0�ɂ���
        canvasGroupInfo.alpha = 0;

        //canvasGroupInfo �� Alpha �̒l�� 0.5�b������ 1.0f �ɕω�������
        canvasGroupInfo.DOFade(1.0f, 0.5f);

        //txtInfo.text��Game Start!�\��
        txtInfo.text = "Game Start!";

        //1�b�҂�
        yield return new WaitForSeconds(1.0f);

        // canvasGroupInfo �� Alpha �̒l�� 0.5�b������ 0.0f �ɕω�������
        canvasGroupInfo.DOFade(0f, 0.5f);

        //canvasGroupTitle��gameObjec���\���ɂ���
        canvasGroupTitle.gameObject.SetActive(false);
    }

}



