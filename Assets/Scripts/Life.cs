using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    private Text lifeText = null;
    private int oldlifeNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        lifeText = GetComponent<Text>();
        if (GManager.instance != null)
        {
            lifeText.text = "× " + GManager.instance.lifeNum;
        }
        else
        {
            Debug.Log("ゲームマネージャー置き忘れてるよ！");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldlifeNum != GManager.instance.lifeNum)
        {
            lifeText.text = "× " + GManager.instance.lifeNum;
            oldlifeNum = GManager.instance.lifeNum;
        }
    }
}
