using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextBtn : MonoBehaviour
{
    [Header("�t�F�[�h")]
    public FadeImage fade;
    //[Header("�Q�[���X�^�[�g���ɖ炷SE")] public AudioClip startSE;
    private bool firstPush = false;
    private bool goNextScene = false;
    public void Next()
    {

        if (!firstPush)
        {
            //GManager.instance.PlaySE(startSE);
            fade.StartFadeOut();
            firstPush = true;
        }
    }
    private void Update()
    {
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("Stage2");
            goNextScene = true;
        }
    }
}
