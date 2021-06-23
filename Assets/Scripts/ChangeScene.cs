using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;    //シーンの切り替えに必要

public class ChangeScene : MonoBehaviour
{
    public string sceneName;  //読み込むシーン
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    /// <summary>
    /// シーンを読み込む
    /// </summary>
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
