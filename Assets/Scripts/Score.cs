using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Text scoreText = null;
    private int oldScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GManager.instance != null)
        {
            scoreText.text = "Score " + GManager.instance.score;
        }
        else
        {
            Debug.Log("�Q�[���}�l�[�W���[�u���Y��Ă��I");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldScore != GManager.instance.score)
        {
            scoreText.text = "Score " + GManager.instance.score;
            oldScore = GManager.instance.score;
        }
    }
    public void UpdateDisplayScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
