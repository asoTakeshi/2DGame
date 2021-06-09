using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    private bool isGoal;                 // ゴールの重複判定防止用。一度ゴール判定したら true にして、ゴールの判定は１回だけしか行わないようにする



    private void OnTriggerEnter2D(Collider2D col)
    {

        // 接触した(ゴールした)際に１回だけ判定する
        if (col.gameObject.tag == "Player" && isGoal == false)
        {

            // ２回目以降はゴール判定を行わないようにするために、true に変更する
            isGoal = true;

            Debug.Log("ゲームクリア");
        }
    }
}

