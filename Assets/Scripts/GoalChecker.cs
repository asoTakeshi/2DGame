using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    private bool isGoal;                 // �S�[���̏d������h�~�p�B��x�S�[�����肵���� true �ɂ��āA�S�[���̔���͂P�񂾂������s��Ȃ��悤�ɂ���



    private void OnTriggerEnter2D(Collider2D col)
    {

        // �ڐG����(�S�[������)�ۂɂP�񂾂����肷��
        if (col.gameObject.tag == "Player" && isGoal == false)
        {

            // �Q��ڈȍ~�̓S�[��������s��Ȃ��悤�ɂ��邽�߂ɁAtrue �ɕύX����
            isGoal = true;

            Debug.Log("�Q�[���N���A");
        }
    }
}

