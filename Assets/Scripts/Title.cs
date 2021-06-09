using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("フェード")] 
    public FadeImage fade;
    //[Header("ゲームスタート時に鳴らすSE")] public AudioClip startSE;
    private bool firstPush = false;
    private bool goNextScene = false;
    // スタートボタンが押されると呼ばれる
    public void PressStart()
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
            SceneManager.LoadScene("Main1");
            goNextScene = true;
        }
    }

}
