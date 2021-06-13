using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextBtn : MonoBehaviour
{
    [Header("フェード")]
    public FadeImage fade;
    //[Header("ゲームスタート時に鳴らすSE")] public AudioClip startSE;
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
