using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManeger : MonoBehaviour
{
    public GameObject impactPrefab;
    float speed = 10f;  //�X�s�[�h�̑��
    Rigidbody2D rb;
    [Header("�U����")]
    public int at;



    public void Shot(float direction)
    {
        //Debug.Log(direction);
        rb = GetComponent<Rigidbody2D>();�@�@�@  //Rigidbody2D�̃R���|�[�l���g��ϐ��ɑ��
        rb.velocity = transform.right * speed * direction;   //����Ԏ������ɑł�

    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        //�G�ɓ���������
        if (col.gameObject.tag == "Enemy")
        {

            //�_���[�W��^����
            EnemyManager enemy = col.gameObject.GetComponent<EnemyManager>();
            enemy.OnDamage(at);

            GameObject effect = Instantiate(impactPrefab, transform.position, transform.rotation);

            Destroy(effect, 1.0f);
            //�j��
            //Destroy(col.gameObject);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
