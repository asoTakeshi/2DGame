using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour
{
    public int index;

    /// <summary>
    /// リザルトポップアップから呼び出し
    /// </summary>
    public void NextStage()
    {
        GManager.instance.stageNum++;
        int index = SceneManager.GetActiveScene().buildIndex;
        index++;
        SceneManager.LoadScene(index);
        Debug.Log(index);
    }
}
